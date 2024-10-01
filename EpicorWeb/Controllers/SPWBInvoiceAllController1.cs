using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EpicorWeb.Controllers
{
    public class SPWBInvoiceAllController1 : Controller
    {
        private static DateTime _Fromdate;
        private static DateTime _Todate;
        private readonly IMemoryCache _memoryCache;

        public SPWBInvoiceAllController1(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/SPWBInvoiceAll")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/SPWBInvoiceAll" }).Rows.Count > 0)
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
        [Route("/SPWBInvoiceAll/SPWBInvoiceAllShow")]
        public IActionResult ShowInvoice(DateTime fromDate, DateTime toDate)
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                if (fromDate == new DateTime(0001, 1, 1) || toDate == new DateTime(0001, 1, 1))
                {
                    TempData["SPWBInvoiceAll_Error"] = "- Vui lòng chọn ngày.";
                    return RedirectToAction("Index", "SPWBInvoiceAll");
                }
                else
                {
                    string query = "exec sp_VNGetInvoice_AllByFromdateTodate @Fromdate , @Todate";
                    DataTable Invoices = new DataProvider().ExecuteQuery(query, new object[] { fromDate, toDate });
                    List<Invoices> InvoicesList = new();
                    _Fromdate = fromDate;
                    _Todate = toDate;
                    foreach (DataRow invoice in Invoices.Rows)
                    {
                        #pragma warning disable CS8604 // Possible null reference argument.
                        Invoices inv = new()
                        {
                            Invoice = int.Parse(invoice["Invoice"].ToString()),
                            Suffix = invoice["Suffix"].ToString(),
                            Date = DateTime.Parse(invoice["Datee"].ToString()).Date,
                            Due_date = DateTime.Parse(invoice["Due_date"].ToString()).Date,
                            Open = bool.Parse(invoice["Openn"].ToString()),
                            Type = invoice["Typee"].ToString(),
                            Name = invoice["Namee"].ToString(),
                            Part = invoice["Part"].ToString(),
                            Description = invoice["Descriptionn"].ToString(),
                            LegalNumber = invoice["LegalNumber"].ToString(),
                            CustInvNum = invoice["CustInvNum"].ToString(),
                            Total_Inv_Amt = invoice["Total_Inv_Amt"].ToString(),
                            Inv_Bal = invoice["Inv_Bal"].ToString(),
                            Currency = invoice["Currency"].ToString(),
                            Inv_Line = invoice["Inv_Line"].ToString(),
                            PO = invoice["PO"].ToString(),
                            PO_Line = invoice["PO_Line"].ToString(),
                            Sales_Order = invoice["Sales_Order"].ToString(),
                            SO_Line = invoice["SO_Line"].ToString(),
                            Line_Amt = invoice["Line_Amt"].ToString(),
                            UR_Check = invoice["UR_Check"].ToString(),
                            Entered_By = invoice["Entered_By"].ToString(),
                            Hold_Invoice = invoice["Hold_Invoice"].ToString()
                        };
                        #pragma warning restore CS8604 // Possible null reference argument.

                        InvoicesList.Add(inv);
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
        [Route("/SPWBInvoiceAll/SPWBInvoiceExportExcel")]
        public IActionResult SPWBInvoiceExportExcel(DateTime startDate, DateTime endDate)
        {
            string query = "exec sp_VNGetInvoice_AllByFromdateTodate @Fromdate , @Todate";
            DataTable Invoices = new DataProvider().ExecuteQuery(query, new object[] { startDate, endDate });
            List<Invoices> InvoicesList = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
            #pragma warning disable CS8604 // Possible null reference argument.
                Invoices inv = new()
                {
                    Invoice = int.Parse(invoice["Invoice"].ToString()),
                    Suffix = invoice["Suffix"].ToString(),
                    Date = DateTime.Parse(invoice["Datee"].ToString()).Date,
                    Due_date = DateTime.Parse(invoice["Due_date"].ToString()).Date,
                    Open = bool.Parse(invoice["Openn"].ToString()),
                    Type = invoice["Typee"].ToString(),
                    Name = invoice["Namee"].ToString(),
                    Part = invoice["Part"].ToString(),
                    Description = invoice["Descriptionn"].ToString(),
                    LegalNumber = invoice["LegalNumber"].ToString(),
                    CustInvNum = invoice["CustInvNum"].ToString(),
                    Total_Inv_Amt = invoice["Total_Inv_Amt"].ToString(),
                    Inv_Bal = invoice["Inv_Bal"].ToString(),
                    Currency = invoice["Currency"].ToString(),
                    Inv_Line = invoice["Inv_Line"].ToString(),
                    PO = invoice["PO"].ToString(),
                    PO_Line = invoice["PO_Line"].ToString(),
                    Sales_Order = invoice["Sales_Order"].ToString(),
                    SO_Line = invoice["SO_Line"].ToString(),
                    Line_Amt = invoice["Line_Amt"].ToString(),
                    UR_Check = invoice["UR_Check"].ToString(),
                    Entered_By = invoice["Entered_By"].ToString(),
                    Hold_Invoice = invoice["Hold_Invoice"].ToString()
                };
                #pragma warning restore CS8604 // Possible null reference argument.

                InvoicesList.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().XuatExcelWithEpplusForInvoiceAll(InvoicesList);

            //// Lưu workbook vào MemoryStream
            //var stream = new MemoryStream(package.GetAsByteArray());

            // Thiết lập tên file
            string fileName = "InvoiceAll.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
