using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class PurchaseInvoiceListVNController1 : Controller
    {
        private static DateTime _Fromdate;
        private static DateTime _Todate;
        private readonly IMemoryCache _memoryCache;

        public PurchaseInvoiceListVNController1(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/PurchaseInvoiceListVN")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/PurchaseInvoiceListVN" }).Rows.Count > 0)
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

        [HttpPost]
        [Route("/PurchaseInvoiceListVN/PurchaseInvoiceListVNShow")]
        public IActionResult ShowPurchaseInvoiceListVn(DateTime fromDate, DateTime toDate)
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                if (fromDate == new DateTime(0001, 1, 1) || toDate == new DateTime(0001, 1, 1))
                {
                    TempData["PurchaseInvoiceListVN_Error"] = "- Vui lòng chọn ngày.";
                    return RedirectToAction("Index", "PurchaseInvoiceListVN");
                }
                else
                {
                    string query = "exec sp_VNPurchaseInvoiceListVN @Fromdate , @Todate";
                    DataTable VNPurchaseInvoiceListVN = new DataProvider().ExecuteQuery(query, new object[] { fromDate, toDate });
                    List<PurchaseInvoiceListVN> InvoicesList = new();
                    _Fromdate = fromDate;
                    _Todate = toDate;
                    foreach (DataRow invoicesList in VNPurchaseInvoiceListVN.Rows)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        PurchaseInvoiceListVN invl = new()
                        {
                            Stt = int.Parse(invoicesList["Stt"].ToString()),
                            InvoiceNum = invoicesList["InvoiceNum"].ToString(),
                            InvoiceDate = DateTime.Parse(invoicesList["InvoiceDate"].ToString()).Date,
                            Applydate = DateTime.Parse(invoicesList["ApplyDate"].ToString()).Date,
                            Name = invoicesList["Namee"].ToString(),
                            TaxPayerID = invoicesList["TaxPayerID"].ToString(),
                            InvoiceAmt = decimal.Parse(invoicesList["InvoiceAmt"].ToString()),
                            Percentt = decimal.Parse(invoicesList["Percentt"].ToString()),
                            TaxAmt = decimal.Parse(invoicesList["TaxAmt"].ToString()),
                            Descriptionn = invoicesList["Descriptionn"].ToString()
                        };
#pragma warning restore CS8604 // Possible null reference argument.

                        InvoicesList.Add(invl);
                    }
                    return View(InvoicesList);

                }
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/PurchaseInvoiceListVN/PurchaseInvoiceListVNExportExcel")]
        public IActionResult PurchaseInvoiceListVNExportExcel(DateTime startDate, DateTime endDate)
        {
            string query = "exec sp_VNPurchaseInvoiceListVN @Fromdate , @Todate";
            DataTable Invoices = new DataProvider().ExecuteQuery(query, new object[] { startDate, endDate });
            List<PurchaseInvoiceListVN> InvoicesList = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                PurchaseInvoiceListVN inv = new()
                {
                    Stt = int.Parse(invoice["Stt"].ToString()),
                    InvoiceNum = invoice["InvoiceNum"].ToString(),
                    InvoiceDate = DateTime.Parse(invoice["InvoiceDate"].ToString()).Date,
                    Applydate = DateTime.Parse(invoice["Applydate"].ToString()).Date,
                    Name = invoice["Namee"].ToString(),
                    TaxPayerID = invoice["TaxPayerID"].ToString(),
                    InvoiceAmt = decimal.Parse(invoice["InvoiceAmt"].ToString()),
                    TaxAmt = decimal.Parse(invoice["TaxAmt"].ToString()),
                    Percentt = decimal.Parse(invoice["Percentt"].ToString()),
                    Descriptionn = invoice["Descriptionn"].ToString()
                };
#pragma warning restore CS8604 // Possible null reference argument.

                InvoicesList.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForPurchaseInvoiceListVN(InvoicesList);

            // Thiết lập tên file
            string fileName = "PurchaseInvoiceListVN.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /// <summary>
        /// Xuất excel cho báo cáo Tham khảo
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/PurchaseInvoiceListVN/PurchaseInvoiceListVNExportExcelTK")]
        public IActionResult PurchaseInvoiceListVNExportExcelTK(DateTime startDate, DateTime endDate)
        {
            string query = "exec sp_VNPurchaseInvoiceListVN_Addition @Fromdate , @Todate";
            DataTable Invoices = new DataProvider().ExecuteQuery(query, new object[] { startDate, endDate });
            List<PurchaseInvoiceListVN> InvoicesList = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                PurchaseInvoiceListVN inv = new()
                {
                    Stt = int.Parse(invoice["Stt"].ToString()),
                    InvoiceNum = invoice["InvoiceNum"].ToString(),
                    InvoiceDate = DateTime.Parse(invoice["InvoiceDate"].ToString()).Date,
                    Applydate = DateTime.Parse(invoice["Applydate"].ToString()).Date,
                    Name = invoice["Namee"].ToString(),
                    TaxPayerID = invoice["TaxPayerID"].ToString(),
                    InvoiceAmt = decimal.Parse(invoice["InvoiceAmt"].ToString()),
                    TaxAmt = decimal.Parse(invoice["TaxAmt"].ToString()),
                    Percentt = decimal.Parse(invoice["Percentt"].ToString()),
                    Descriptionn = invoice["Descriptionn"].ToString(),
                    VendorName_c = invoice["VendorName_c"].ToString(),
                    TaxID_c = invoice["TaxID_c"].ToString(),
                    VendorAddress_c = invoice["VendorAddress_c"].ToString()
                };
#pragma warning restore CS8604 // Possible null reference argument.

                InvoicesList.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForPurchaseInvoiceListVNTK(InvoicesList);

            // Thiết lập tên file
            string fileName = "PurchaseInvoiceListVNTK.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
