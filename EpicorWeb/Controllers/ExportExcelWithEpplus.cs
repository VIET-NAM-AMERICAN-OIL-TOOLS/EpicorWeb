using OfficeOpenXml;
using System.Diagnostics;
using FastMember;
using System.Data;
using OfficeOpenXml.Style;
using System.Drawing;

namespace EpicorWeb.Controllers
{
    public class ExportExcelWithEpplus
    {
        /// <summary>
        /// Xuất excel cho Issue Misc Mtl
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        public byte[] ExportExcelWithEpplusForIssueMiscMtl<T>(List<T> data)
        {
            // Chuyển đổi danh sách thành DataTable
            DataTable table = ConvertToDataTable(data);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Columns[i].Width = 16;
            }
            //worksheet.Cells["A1:A500"].Style.Numberformat.Format = "#,##0";
            //worksheet.Cells.LoadFromDataTable(table, false, OfficeOpenXml.Table.TableStyles.Medium2);
            worksheet.Cells["B:B"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["D:D"].Style.Numberformat.Format = "dd/MM/yyyy";

            // Trích xuất danh sách tên cột từ dữ liệu (data)
            var properties = typeof(T).GetProperties();
            List<string> columnHeaders = new List<string>();
            foreach (var property in properties)
            {
                columnHeaders.Add(property.Name);
            }

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = columnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = columnHeaders[i].ToUpper();
            }


            int row = 2; // Bắt đầu từ dòng thứ hai
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value;
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Xuất excel cho hóa đơn
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        public byte[] XuatExcelWithEpplusForInvoiceAll<T>(List<T> data)
        {
            // Chuyển đổi danh sách thành DataTable
            DataTable table = ConvertToDataTable(data);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Columns[i].Width = 16;
            }
            //worksheet.Cells["A1:A500"].Style.Numberformat.Format = "#,##0";
            //worksheet.Cells.LoadFromDataTable(table, false, OfficeOpenXml.Table.TableStyles.Medium2);
            worksheet.Cells["C:C"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["D:D"].Style.Numberformat.Format = "dd/MM/yyyy";

            // Trích xuất danh sách tên cột từ dữ liệu (data)
            var properties = typeof(T).GetProperties();
            List<string> columnHeaders = new List<string>();
            foreach (var property in properties)
            {
                columnHeaders.Add(property.Name);
            }

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = columnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = columnHeaders[i].ToUpper();
            }


            int row = 2; // Bắt đầu từ dòng thứ hai
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value;
                }
                row++;
            }

            using MemoryStream memoryStream = new ();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public DataTable ConvertToDataTable<T>(List<T> data)
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data))
            {
                table.Load(reader);
            }
            return table;
        }

        /// <summary>
        /// Xuất excel Purchase Invoice List VN
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        public byte[] ExportExcelWithEpplusForPurchaseInvoiceListVN<T>(List<T> data)
        {
            // Chuyển đổi danh sách thành DataTable
            DataTable table = ConvertToDataTable(data);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Columns[i].Width = 16;
            }
            //worksheet.Cells["A1:A500"].Style.Numberformat.Format = "#,##0";
            //worksheet.Cells.LoadFromDataTable(table, false, OfficeOpenXml.Table.TableStyles.Medium2);
            worksheet.Cells["C:C"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["D:D"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["G:G"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["H:H"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["I:I"].Style.Numberformat.Format = "#,###,###,###,###.##0";

            // Trích xuất danh sách tên cột từ dữ liệu (data)
            var properties = typeof(T).GetProperties();
            List<string> columnHeaders = new List<string>();
            foreach (var property in properties)
            {
                columnHeaders.Add(property.Name);
            }

            string[] fixedColumnHeaders = { "Sequence No", "Invoice No.", "Invoice Date",
            "Apply Date", "Supplier Name", "Supplier Business Tax Registration No",
                "Purchase Amount (Without Tax)", "Tax Amount", "Tax Percent", "Description"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }


            int row = 2; // Bắt đầu từ dòng thứ hai
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value;
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }


        /// <summary>
        /// Xuất excel Vinam Purchase Invoice List
        /// </summary>
        public byte[] ExportExcelWithEpplusForVinamPurchaseInvoiceList<T>(List<T> data)
        {
            // Chuyển đổi danh sách thành DataTable
            DataTable table = ConvertToDataTable(data);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Columns[i].Width = 16;
            }
            //worksheet.Cells["A1:A500"].Style.Numberformat.Format = "#,##0";
            //worksheet.Cells.LoadFromDataTable(table, false, OfficeOpenXml.Table.TableStyles.Medium2);
            worksheet.Cells["I:I"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["J:J"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["K:K"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["N:N"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["O:O"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["P:P"].Style.Numberformat.Format = "#,###,###,###,###.##0";

            // Trích xuất danh sách tên cột từ dữ liệu (data)
            var properties = typeof(T).GetProperties();
            List<string> columnHeaders = new List<string>();
            //foreach (var property in properties)
            //{
            //    columnHeaders.Add(property.Name);
            //}

            string[] fixedColumnHeaders = { "STT", "Company Name", "Business Tax Registration No:",
            "Currency", "Invoice Type", "Invoice Head", "Legal Number", "Invoice No.", "Invoice Date",
                "Apply Date", "Report Date", "Supplier Name", "Supplier Business Tax Registration No",
                "Purchase Amount (Without Tax)",  "Tax Percent",  "Tax Amount",  "Description"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }


            int row = 2; // Bắt đầu từ dòng thứ hai
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value;
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Xuất excel Price List
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        public byte[] ExportExcelWithEpplusForPriceList<T>(List<T> data)
        {
            // Chuyển đổi danh sách thành DataTable
            DataTable table = ConvertToDataTable(data);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Columns[1].Width = 16;
                worksheet.Columns[2].Width = 16;
                worksheet.Columns[3].Width = 16;
                worksheet.Columns[4].Width = 16;
                worksheet.Columns[5].Width = 16;
                worksheet.Columns[6].Width = 16;
                worksheet.Columns[7].Width = 40;
            }
            //worksheet.Cells["A1:A500"].Style.Numberformat.Format = "#,##0";
            //worksheet.Cells.LoadFromDataTable(table, false, OfficeOpenXml.Table.TableStyles.Medium2);
            worksheet.Column(1).Style.Numberformat.Format = "@";
            worksheet.Column(2).Style.Numberformat.Format = "@";
            worksheet.Column(3).Style.Numberformat.Format = "@";
            worksheet.Column(4).Style.Numberformat.Format = "#,###,###,###.####0";
            worksheet.Column(5).Style.Numberformat.Format = "#,###,###,###.####0";
            worksheet.Column(6).Style.Numberformat.Format = "#,###,###,###.####0";
            worksheet.Column(7).Style.Numberformat.Format = "@";

            // Trích xuất danh sách tên cột từ dữ liệu (data)
            var properties = typeof(T).GetProperties();
            List<string> columnHeaders = new List<string>();
            foreach (var property in properties)
            {
                columnHeaders.Add(property.Name);
            }

            // Định dạng dòng đầu tiên là kiểu chuỗi (string)
            worksheet.Row(1).Style.Numberformat.Format = "@";
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Font.Size = 12;

            string[] fixedColumnHeaders = { "Company", "ListCode", "PartNum", "Quantity", "BasePrice", "UnitPrice","SysRowID"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }


            int row = 2; // Bắt đầu từ dòng thứ hai
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value;
                }
                row++;
            }

            // Tô màu cho các cột (ví dụ: cột 1 và 2 có màu xanh)
            int[] coloredColumns = { 1, 2, 3, 4 }; // Chỉ số cột bạn muốn tô màu
            foreach (int columnIndex in coloredColumns)
            {
                using (var range = worksheet.Cells[1, columnIndex, table.Rows.Count + 1, columnIndex])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.PaleTurquoise);
                }
            }

            int[] coloredColumns1 = { 7 }; // Chỉ số cột bạn muốn tô màu
            foreach (int columnIndex in coloredColumns1)
            {
                using (var range = worksheet.Cells[1, columnIndex, table.Rows.Count + 1, columnIndex])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
                }
            }

            // Thêm border (kẻ dòng) cho toàn bộ dữ liệu
            using (var range = worksheet.Cells[1, 1, table.Rows.Count + 1, table.Columns.Count])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }


            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Xuất excel Purchase Invoice List VN Tham khảo
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        public byte[] ExportExcelWithEpplusForPurchaseInvoiceListVNTK<T>(List<T> data)
        {
            // Chuyển đổi danh sách thành DataTable
            DataTable table = ConvertToDataTable(data);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Columns[i].Width = 16;
            }
            //worksheet.Cells["A1:A500"].Style.Numberformat.Format = "#,##0";
            //worksheet.Cells.LoadFromDataTable(table, false, OfficeOpenXml.Table.TableStyles.Medium2);
            worksheet.Cells["C:C"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["D:D"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["G:G"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["H:H"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["I:I"].Style.Numberformat.Format = "#,###,###,###,###.##0";

            // Trích xuất danh sách tên cột từ dữ liệu (data)
            var properties = typeof(T).GetProperties();
            List<string> columnHeaders = new List<string>();
            foreach (var property in properties)
            {
                columnHeaders.Add(property.Name);
            }

            string[] fixedColumnHeaders = { "Sequence No", "Invoice No.", "Invoice Date",
            "Apply Date", "Supplier Name", "Supplier Business Tax Registration No",
                "Purchase Amount (Without Tax)", "Tax Amount", "Tax Percent", "Description",
            "Vendor", "Vendor's Tax Code", "Vendor's Address"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }


            int row = 2; // Bắt đầu từ dòng thứ hai
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value;
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Xuất excel Vinam Purchase Invoice List Tham Khảo
        /// </summary>
        public byte[] ExportExcelWithEpplusForVinamPurchaseInvoiceListTK<T>(List<T> data)
        {
            // Chuyển đổi danh sách thành DataTable
            DataTable table = ConvertToDataTable(data);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Columns[i].Width = 16;
            }
            //worksheet.Cells["A1:A500"].Style.Numberformat.Format = "#,##0";
            //worksheet.Cells.LoadFromDataTable(table, false, OfficeOpenXml.Table.TableStyles.Medium2);
            worksheet.Cells["I:I"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["J:J"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["K:K"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["N:N"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["O:O"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["P:P"].Style.Numberformat.Format = "#,###,###,###,###.##0";

            // Trích xuất danh sách tên cột từ dữ liệu (data)
            var properties = typeof(T).GetProperties();
            List<string> columnHeaders = new List<string>();
            //foreach (var property in properties)
            //{
            //    columnHeaders.Add(property.Name);
            //}

            string[] fixedColumnHeaders = { "STT", "Company Name", "Business Tax Registration No:",
            "Currency", "Invoice Type", "Invoice Head", "Legal Number", "Invoice No.", "Invoice Date",
                "Apply Date", "Report Date", "Supplier Name", "Supplier Business Tax Registration No",
                "Purchase Amount (Without Tax)",  "Tax Percent",  "Tax Amount",  "Description",
            "Vendor", "Vendor's Tax Code", "Vendor's Address"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }


            int row = 2; // Bắt đầu từ dòng thứ hai
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value;
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Xuất excel Vinam NCR không date
        /// </summary>
        public byte[] ExportExcelWithEpplusForVinamNCR(DataTable table)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Column(i).Width = 20;
                worksheet.Column(2).Width = 24;
                worksheet.Column(3).Width = 24;
                worksheet.Column(4).Width = 24;
                worksheet.Column(5).Width = 24;
                worksheet.Column(6).Width = 24;
            }

            worksheet.Cells["B:B"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["C:C"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["D:D"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["E:E"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["F:F"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["J:J"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["K:K"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["N:N"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            //worksheet.Cells["O:O"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            //worksheet.Cells["P:P"].Style.Numberformat.Format = "#,###,###,###,###.##0";

            string[] fixedColumnHeaders = { "STT", "SYS DATE(dd/MM/yyyy)", "DATE OPEN(dd/MM/yyyy)" , "ACTION COMP(dd/MM/yyyy)" , "DUDATE(dd/MM/yyyy)" , "AUDIT DATE(dd/MM/yyyy)" , 
                "SITE","INSPECTION PENDING","TRAN ID","EMPLOYEE","NAME","JOB NUMBER",
            "PART", "OPR", "OPERATION", "DESCRIPTION", "DMR NUMBER", "ACTION ID","PASSED QTY","SCRAP QTY","COMMENT TEXT","NCR-QUANTITY",
            "CAUSE REASON", "CORRECTIVE ACTION REASON", "ROOT CAUSE INVESTIGATION", "COMMENT TEXT","AUDIT COMMENTS","STATUS CODE"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }

            // Load data from DataTable to Excel worksheet, starting from row 2
            int row = 2;
            foreach (DataRow dataRow in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    worksheet.Cells[row, i + 1].Value = dataRow[i];
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Xuất excel Vinam NCR không date
        /// </summary>
        public byte[] ExportExcelWithEpplusForVinamNCRCustomer(DataTable table)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Column(i).Width = 20;
                worksheet.Column(10).Width = 28;
                worksheet.Column(13).Width = 28;
                worksheet.Column(21).Width = 28;
                worksheet.Column(26).Width = 28;
            }

            worksheet.Cells["J:J"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["M:M"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["U:U"].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells["Z:Z"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["J:J"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["K:K"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["N:N"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            //worksheet.Cells["O:O"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            //worksheet.Cells["P:P"].Style.Numberformat.Format = "#,###,###,###,###.##0";

            string[] fixedColumnHeaders = { "STT", "CASE NUMBER", "TOPIC ID" , "CASE DESCRIPTION" , "CUSTOMER NAME","ORDER",
                "PART","COMPLAINT QUALITY","CONTRACT","CREATED DATE(dd/MM/yyyy)","CREATED BY","DESCRIPTION","COMPLETION DATE(dd/MM/yyyy)",
                "COMPLETION BY","CASE STATUS", "RESOLUTION", "STAGE ID", "LEGAL NUMBER", "LINE", "RETURN","RMA DATE(dd/MM/yyyy)",
                "RMA STATUS","RETURN QUANTITY", "RETURN REASON", "RECEIVED QUATITY", "RECEIVED DATE(dd/MM/yyyy)","DISPOSITION TYPE",
                "DISPOSITION QUATITY","DISPOSITION REASON CODE","RVC"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }

            // Load data from DataTable to Excel worksheet, starting from row 2
            int row = 2;
            foreach (DataRow dataRow in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    worksheet.Cells[row, i + 1].Value = dataRow[i];
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Xuất excel Vinam NCR không date
        /// </summary>
        public byte[] ExportExcelWithEpplusForVinamRawSlowMoving(DataTable table)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Tạo tập tin Excel với chế độ đệm
            using ExcelPackage excel = new();
            excel.Workbook.Worksheets.Add("Sheet1");
            var worksheet = excel.Workbook.Worksheets["Sheet1"];
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                worksheet.Column(i).Width = 20;
                worksheet.Column(2).Width = 24;
                worksheet.Column(3).Width = 24;
                worksheet.Column(4).Width = 24;
                worksheet.Column(5).Width = 24;
                worksheet.Column(6).Width = 24;
            }

            worksheet.Cells["E:E"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["F:F"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["G:G"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["H:H"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            worksheet.Cells["I:I"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            //worksheet.Cells["J:J"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["K:K"].Style.Numberformat.Format = "dd/MM/yyyy";
            //worksheet.Cells["N:N"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            //worksheet.Cells["O:O"].Style.Numberformat.Format = "#,###,###,###,###.##0";
            //worksheet.Cells["P:P"].Style.Numberformat.Format = "#,###,###,###,###.##0";

            string[] fixedColumnHeaders = { "PART", "DESCRIPTION", "WAREHOUSE" , "LOT NUM" , "BEGIN QUANTITY" , "RECEIVED QUANTITY" ,
                "ISSUED QUANTITY","END QUANTITY","END AMOUNT USD","CLASS ID"};

            // Bổ sung tiêu đề cho các cột
            for (int i = 0; i < fixedColumnHeaders.Length; i++)
            {
                var headerCell = worksheet.Cells[1, i + 1];
                headerCell.Value = fixedColumnHeaders[i];
                headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkCyan);
                headerCell.Value = fixedColumnHeaders[i].ToUpper();
            }

            // Load data from DataTable to Excel worksheet, starting from row 2
            int row = 2;
            foreach (DataRow dataRow in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    worksheet.Cells[row, i + 1].Value = dataRow[i];
                }
                row++;
            }

            using MemoryStream memoryStream = new();
            excel.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
