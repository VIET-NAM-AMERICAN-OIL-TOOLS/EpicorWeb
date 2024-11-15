using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class VinamSupplierPriceListController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public VinamSupplierPriceListController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/VinamSupplierPriceList")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/VinamSupplierPriceList" }).Rows.Count > 0)
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
        [Route("/VinamSupplierPriceList/VinamSupplierPriceListExportExcel")]
        public IActionResult VinamSupplierPriceListExportExcel()
        {
            string query = "exec SP_GetSupplierPriceList";
            DataTable Invoices = new DataProviderLocal().ExecuteQuery(query);
            List<SupplierPriceList> InvoicesList = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                SupplierPriceList inv = new()
                {
                    Company = invoice["Company"].ToString(),
                    PartNum = invoice["PartNum"].ToString(),
                    VendorNum = int.Parse(invoice["VendorNum"].ToString()),
                    EffectiveDate = DateTime.Parse(invoice["EffectiveDate"].ToString()),
                    BreakQty = Decimal.Parse(invoice["BreakQty"].ToString()),
                    PriceModifier = Decimal.Parse(invoice["PriceModifier"].ToString()),
                    SysRowID = Guid.Parse(invoice["SysRowID"].ToString())
                };
#pragma warning restore CS8604 // Possible null reference argument.

                InvoicesList.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForPriceList1(InvoicesList);

            // Thiết lập tên file
            string fileName = "VinamSupplierPriceList.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        [Route("/VinamSupplierPriceList/SupplierPriceListVNImportExcel")]
        public async Task<int> SupplierPriceListVNImportExcel(IFormFile file)
        {
            int dem = 0;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int i = 2; i <= rowcount; i++)
                    {
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        decimal breakQty = decimal.Parse(worksheet.Cells[i, 5].Value.ToString().Trim());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        decimal priceModifier = decimal.Parse(worksheet.Cells[i, 6].Value.ToString().Trim());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        Guid sysRowID = Guid.Parse(worksheet.Cells[i, 7].Value.ToString().Trim());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        string query = "exec [dbo].SP_UpdateVendPBrk @SysRowID , @BreakQty , @PriceModifier";
                        dem = dem + new DataProviderLocal().ExecuteNonQuery(query, new object[] { sysRowID , breakQty , priceModifier });
                    }
                }
            }
            return dem;
        }
    }
}
