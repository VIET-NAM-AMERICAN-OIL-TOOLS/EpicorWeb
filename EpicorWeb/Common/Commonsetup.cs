using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EpicorWeb.Attributes;
using FastMember;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace EpicorWeb.Common
{
	public static class Commonsetup
	{
		public static List<T> ConvertDataTableToObject<T>(DataTable dt)
		{
			List<T> data = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T item = GetItemPDM<T>(row);
				data.Add(item);
			}
			return data;
		}
		private static T GetItemPDM<T>(DataRow dr)
		{
			Type temp = typeof(T);
			T obj = Activator.CreateInstance<T>();

			try
			{
				foreach (DataColumn column in dr.Table.Columns)
				{
					foreach (PropertyInfo pro in temp.GetProperties())
					{
						if (pro.Name == column.ColumnName)
						{
							try
							{
								object value = dr[column.ColumnName];
								if (value != DBNull.Value)
								{
									// Xử lý kiểu số nguyên có nullable
									if (Nullable.GetUnderlyingType(pro.PropertyType) != null && pro.PropertyType == typeof(int?))
									{
										if (value is long || value is int)
										{
											pro.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(pro.PropertyType)), null);
										}
										else
										{
											pro.SetValue(obj, null, null);
										}
									}
									else if (pro.PropertyType == typeof(float) || pro.PropertyType == typeof(double) || pro.PropertyType == typeof(int) || pro.PropertyType == typeof(decimal))
									{
										pro.SetValue(obj, Convert.ChangeType(value, pro.PropertyType), null);
									}
									// Xử lý kiểu bool và bool?
									else if (pro.PropertyType == typeof(bool) || pro.PropertyType == typeof(bool?))
									{
										bool? boolValue = value as bool?;
										if (boolValue == null && value is SByte sbyteValue)
										{
											boolValue = sbyteValue == 1 ? true : (sbyteValue == 0 ? (bool?)false : null);
										}
										pro.SetValue(obj, boolValue, null);
									}
									// Xử lý kiểu DateTime và DateTime?
									else if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
									{
										if (pro.PropertyType == typeof(System.DateTime))
										{
											// Chuyển đổi giá trị sang DateOnly
											if (value is DateTime dateTimeValue)
											{
												pro.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType), null);
											}
											else
											{
												pro.SetValue(obj, null, null);
											}
										}
										else
										{
											// Chuyển đổi giá trị sang DateTime hoặc DateTime?
											pro.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType), null);
										}
									}
									else
									{
										pro.SetValue(obj, value, null);
									}
								}
								else
								{
									pro.SetValue(obj, null, null);
								}
							}
							catch (Exception ex)
							{
								var prop = pro.Name;
								// Xử lý lỗi chuyển đổi kiểu dữ liệu
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Xử lý lỗi chung
			}

			return obj;
		}

		public static DataTable ConvertToDataTable<T>(List<T> items)
		{
			DataTable dataTable = new DataTable(typeof(T).Name);

			// Lấy tất cả các thuộc tính của T
			PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// Tạo các cột trong DataTable dựa trên các thuộc tính của T
			foreach (var prop in props)
			{
				var isConvert = (ConvertToDataTableColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(ConvertToDataTableColumnAttribute));
				if (isConvert != null && isConvert.IsConvert)
				{
					dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
				}
			}

			// Thêm các hàng vào DataTable
			foreach (var item in items)
			{
				DataRow row = dataTable.NewRow();
				foreach (var prop in props)
				{
					var isConvert = (ConvertToDataTableColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(ConvertToDataTableColumnAttribute));
					if (isConvert != null && isConvert.IsConvert)
					{
						if (dataTable.Columns.Contains(prop.Name))
						{
							row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
						}
					}
				}
				dataTable.Rows.Add(row);
			}

			return dataTable;
		}

		public static DataTable ConvertToDataTableAll<T>(List<T> data)
		{
			DataTable table = new DataTable();
			using (var reader = ObjectReader.Create(data))
			{
				table.Load(reader);
			}
			return table;
		}

		public static byte[] ExportDataTableToExcel(DataTable dataTable, string fileName)
		{
			try
			{

				ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

				// Tạo tập tin Excel với chế độ đệm
				using ExcelPackage excel = new();
				var worksheet = excel.Workbook.Worksheets.Add("Sheet1");

				// Ghi tiêu đề cột
				for (int col = 0; col < dataTable.Columns.Count; col++)
				{
					worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
				}

				// Ghi dữ liệu vào trang Excel
				for (int row = 0; row < dataTable.Rows.Count; row++)
				{
					for (int col = 0; col < dataTable.Columns.Count; col++)
					{
						worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
					}
				}

				// Định dạng tiêu đề cột
				using (var range = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
				{
					range.Style.Font.Bold = true;
					range.Style.Fill.PatternType = ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
				}

				// Lưu tệp Excel
				using MemoryStream memoryStream = new();
				excel.SaveAs(memoryStream);
				return memoryStream.ToArray();

			}
			catch (Exception ex)
			{
				return null;
			}

		}

		public static byte[] ExportByteToExcel(DataTable dataTable)
		{
			try
			{
				using (var workbook = new XLWorkbook())
				{
					// Thêm DataTable vào worksheet mới
					var worksheet = workbook.Worksheets.Add(dataTable, "Sheet1");

					// Điều chỉnh chiều rộng và chiều cao các cột và hàng
					worksheet.Columns().AdjustToContents();
					worksheet.Rows().AdjustToContents();

					// Tạo một MemoryStream để ghi nội dung workbook
					using (MemoryStream memoryStream = new MemoryStream())
					{
						// Ghi workbook vào MemoryStream
						workbook.SaveAs(memoryStream);
						// Trả về mảng byte từ MemoryStream
						return memoryStream.ToArray();
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
