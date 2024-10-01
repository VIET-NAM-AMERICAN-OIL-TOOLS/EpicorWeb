using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class VinamPriceListController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public VinamPriceListController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/VinamPriceList")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/VinamPriceList" }).Rows.Count > 0)
                {
                    return View();
                }
                else
                {
                    return BadRequest("Bạn không có quyền đăng nhập trang này");
                }
                
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/VinamPriceList/VinamPriceListExportExcel")]
        public IActionResult PriceListVNExportExcel()
        {
            string query = "exec VNSN_GetListUnitPrice";
            DataTable Invoices = new DataProvider().ExecuteQuery(query);
            List<PriceList> InvoicesList = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                PriceList inv = new()
                {
                    Company = invoice["Company"].ToString(),
                    ListCode = invoice["ListCode"].ToString(),
                    PartNum = invoice["PartNum"].ToString(),
                    Quantity = Decimal.Parse(invoice["Quantity"].ToString()),
                    BasePrice = Decimal.Parse(invoice["BasePrice"].ToString()),
                    UnitPrice = Decimal.Parse(invoice["UnitPrice"].ToString()),
                    SysRowID = Guid.Parse(invoice["SysRowID"].ToString())
                };
#pragma warning restore CS8604 // Possible null reference argument.

                InvoicesList.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForPriceList(InvoicesList);

            // Thiết lập tên file
            string fileName = "VinamPriceList.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        [Route("/VinamPriceList/PriceListVNImportExcel")]
        public async Task<int> PriceListVNImportExcel(IFormFile file)
        {
            int dem = 0;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using(var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int i = 2; i <= rowcount; i++)
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        string ListCode = worksheet.Cells[i, 2].Value.ToString().Trim();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        string PartNum = worksheet.Cells[i, 3].Value.ToString().Trim();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        decimal BasePrice = decimal.Parse(worksheet.Cells[i, 5].Value.ToString().Trim());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        decimal UnitPrice = decimal.Parse(worksheet.Cells[i, 6].Value.ToString().Trim());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        Guid SysRowID = Guid.Parse(worksheet.Cells[i, 7].Value.ToString().Trim());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        string query = "exec [dbo].[VNSN_UpdateUnitPrice] @SysRowID , @UnitPrice";
                        string query1 = "exec [dbo].[VNSN_UpdateBasePrice] @ListCode , @PartNum , @BasePrice";
                        dem = dem + new DataProvider().ExecuteNonQuery(query, new object[] {SysRowID , UnitPrice});
                        new DataProvider().ExecuteNonQuery(query1, new object[] {ListCode , PartNum , BasePrice});
                    }
                }
            }
            return dem;
        }
    }
}
