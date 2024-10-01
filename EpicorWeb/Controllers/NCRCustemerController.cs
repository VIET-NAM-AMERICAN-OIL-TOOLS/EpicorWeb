using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class NCRCustemerController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public NCRCustemerController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/NCRCustomer")]
        public IActionResult Index()
        {
            string query = "exec SP_GetNCRCustomer";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query);
            //foreach (DataRow row in dataTable.Rows)
            //{
            //    if (DateTime.TryParse(row["HDCase_CompletionDate"].ToString(), out DateTime NonConf_SysDate))
            //    {
            //        row["HDCase_CompletionDate"] = NonConf_SysDate; // Storing as DateTime
            //    }
            //    if (decimal.TryParse(row["NonConf_PassedQty"].ToString(), out decimal NonConf_PassedQty))
            //    {
            //        row["NonConf_PassedQty"] = NonConf_PassedQty.ToString("0.0");
            //    }
            //    if (decimal.TryParse(row["NonConf_FailedQty"].ToString(), out decimal NonConf_FailedQty))
            //    {
            //        row["NonConf_FailedQty"] = NonConf_FailedQty.ToString("0.0");
            //    }
            //    if (decimal.TryParse(row["NonConf_Quantity"].ToString(), out decimal NonConf_Quantity))
            //    {
            //        row["NonConf_Quantity"] = NonConf_Quantity.ToString("0.0");
            //    }
            //}
            return View(dataTable);
        }

        [HttpGet]
        [Route("/NCRCustomer/NCRCustomerByDate")]
        public IActionResult NCRCustomerByDate(DateTime fromDate, DateTime toDate)
        {
            string query = "exec SP_GetNCRCustomerByDate @FromDate , @ToDate";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query, new object[] { fromDate, toDate });
            
            ViewBag.fromDate = fromDate.Date;
            ViewBag.toDate = toDate.Date;
            return View(dataTable);
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/NCRCustomer/ExportExcel")]
        public IActionResult ExportExcel()
        {
            string query = "exec SP_GetNCRCustomer";
            DataTable Invoices = new DataProviderLocal().ExecuteQuery(query);

            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForVinamNCRCustomer(Invoices);


            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// Xuất excel theo ngày
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/NCRCustomer/ExportExcelByDate")]
        public IActionResult ExportExcelByDate(DateTime fromDate, DateTime toDate)
        {
            string query = "Exec [dbo].[SP_GetNCRCustomerByDate] @FromDate , @ToDate";
            DataTable Invoices = new DataProviderLocal().ExecuteQuery(query, new object[] { fromDate, toDate });

            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForVinamNCRCustomer(Invoices);


            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
