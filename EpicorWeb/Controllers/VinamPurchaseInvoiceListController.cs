using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class VinamPurchaseInvoiceListController : Controller
    {
        private static DateTime _Fromdate;
        private static DateTime _Todate;
        private readonly IMemoryCache _memoryCache;

        public VinamPurchaseInvoiceListController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/VinamPurchaseInvoiceList")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/VinamPurchaseInvoiceList" }).Rows.Count > 0)
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
        [Route("/VinamPurchaseInvoiceList/VinamPurchaseInvoiceListShow")]
        public IActionResult ShowVinamPurchaseInvoiceList(DateTime fromDate, DateTime toDate)
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                if (fromDate == new DateTime(0001, 1, 1) || toDate == new DateTime(0001, 1, 1))
                {
                    TempData["VinamPurchaseInvoiceListShow_Error"] = "- Vui lòng chọn ngày.";
                    return RedirectToAction("Index", "VinamPurchaseInvoiceListShow");
                }
                else
                {
                    string query = "exec sp_VinamPurchaseInvoiceList @Fromdate , @Todate";
                    DataTable VinamPurchaseInvoiceList = new DataProvider().ExecuteQuery(query, new object[] { fromDate, toDate });
                    List<VinamPurchaseInvoiceList> InvoicesList = new();
                    _Fromdate = fromDate;
                    _Todate = toDate;
                    foreach (DataRow invoicesList in VinamPurchaseInvoiceList.Rows)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        VinamPurchaseInvoiceList invl = new()
                        {
                            Stt = int.Parse(invoicesList["Stt"].ToString()),
                            Company = invoicesList["Company"].ToString(),
                            StateTaxID = invoicesList["StateTaxID"].ToString(),
                            Currency = invoicesList["Currency"].ToString(),
                            DataCharacter = invoicesList["DataCharacter"].ToString(),
                            InvoiceHead = invoicesList["InvoiceHead"].ToString(),
                            LegalNumber = invoicesList["LegalNumber"].ToString(),
                            InvoiceNum = invoicesList["InvoiceNum"].ToString(),
                            InvoiceDate = DateTime.Parse(invoicesList["InvoiceDate"].ToString()).Date,
                            Applydate = DateTime.Parse(invoicesList["ApplyDate"].ToString()).Date,
                            ReportDate_c = DateTime.Parse(invoicesList["ReportDate_c"].ToString()).Date,
                            Namee = invoicesList["Namee"].ToString(),
                            TaxPayerID = invoicesList["TaxPayerID"].ToString(),
                            InvoiceAmt = decimal.Parse(invoicesList["InvoiceAmt"].ToString()),
                            Percentt = decimal.Parse(invoicesList["Percentt"].ToString()),
                            TaxAmt = decimal.Parse(invoicesList["TaxAmt"].ToString()),
                            Description_c = invoicesList["Description_c"].ToString()
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
        [Route("/VinamPurchaseInvoiceList/VinamPurchaseInvoiceListExportExcel")]
        public IActionResult VinamPurchaseInvoiceListExportExcel(DateTime startDate, DateTime endDate)
        {
            string query = "exec sp_VinamPurchaseInvoiceList @Fromdate , @Todate";
            DataTable Invoices = new DataProvider().ExecuteQuery(query, new object[] { startDate, endDate });
            List<VinamPurchaseInvoiceList> InvoicesList = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                VinamPurchaseInvoiceList inv = new()
                {
                    Stt = int.Parse(invoice["Stt"].ToString()),
                    Company = invoice["Company"].ToString(),
                    StateTaxID = invoice["StateTaxID"].ToString(),
                    Currency = invoice["Currency"].ToString(),
                    DataCharacter = invoice["DataCharacter"].ToString(),
                    InvoiceHead = invoice["InvoiceHead"].ToString(),
                    LegalNumber = invoice["LegalNumber"].ToString(),
                    InvoiceNum = invoice["InvoiceNum"].ToString(),
                    InvoiceDate = DateTime.Parse(invoice["InvoiceDate"].ToString()).Date,
                    Applydate = DateTime.Parse(invoice["Applydate"].ToString()).Date,
                    ReportDate_c = DateTime.Parse(invoice["ReportDate_c"].ToString()).Date,
                    Namee = invoice["Namee"].ToString(),
                    TaxPayerID = invoice["TaxPayerID"].ToString(),
                    InvoiceAmt = decimal.Parse(invoice["InvoiceAmt"].ToString()),
                    Percentt = decimal.Parse(invoice["Percentt"].ToString()),
                    TaxAmt = decimal.Parse(invoice["TaxAmt"].ToString()),
                    Description_c = invoice["Description_c"].ToString()
                };
#pragma warning restore CS8604 // Possible null reference argument.

                InvoicesList.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForVinamPurchaseInvoiceList(InvoicesList);

            // Thiết lập tên file
            string fileName = "VinamPurchaseInvoiceList.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /// <summary>
        /// Xuất excel Tham Khảo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/VinamPurchaseInvoiceList/VinamPurchaseInvoiceListExportExcelTK")]
        public IActionResult VinamPurchaseInvoiceListExportExcelTK(DateTime startDate, DateTime endDate)
        {
            string query = "exec sp_VinamPurchaseInvoiceList_Addition @Fromdate , @Todate";
            DataTable Invoices = new DataProvider().ExecuteQuery(query, new object[] { startDate, endDate });
            List<VinamPurchaseInvoiceList> InvoicesList = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                VinamPurchaseInvoiceList inv = new()
                {
                    Stt = int.Parse(invoice["Stt"].ToString()),
                    Company = invoice["Company"].ToString(),
                    StateTaxID = invoice["StateTaxID"].ToString(),
                    Currency = invoice["Currency"].ToString(),
                    DataCharacter = invoice["DataCharacter"].ToString(),
                    InvoiceHead = invoice["InvoiceHead"].ToString(),
                    LegalNumber = invoice["LegalNumber"].ToString(),
                    InvoiceNum = invoice["InvoiceNum"].ToString(),
                    InvoiceDate = DateTime.Parse(invoice["InvoiceDate"].ToString()).Date,
                    Applydate = DateTime.Parse(invoice["Applydate"].ToString()).Date,
                    ReportDate_c = DateTime.Parse(invoice["ReportDate_c"].ToString()).Date,
                    Namee = invoice["Namee"].ToString(),
                    TaxPayerID = invoice["TaxPayerID"].ToString(),
                    InvoiceAmt = decimal.Parse(invoice["InvoiceAmt"].ToString()),
                    Percentt = decimal.Parse(invoice["Percentt"].ToString()),
                    TaxAmt = decimal.Parse(invoice["TaxAmt"].ToString()),
                    Description_c = invoice["Description_c"].ToString(),
                    VendorName_c = invoice["VendorName_c"].ToString(),
                    TaxID_c = invoice["TaxID_c"].ToString(),
                    VendorAddress_c = invoice["VendorAddress_c"].ToString()
                };
#pragma warning restore CS8604 // Possible null reference argument.

                InvoicesList.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForVinamPurchaseInvoiceListTK(InvoicesList);

            // Thiết lập tên file
            string fileName = "VinamPurchaseInvoiceListTK.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
