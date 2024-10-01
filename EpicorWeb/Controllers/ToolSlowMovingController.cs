using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class ToolSlowMovingController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ToolSlowMovingController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/ToolSlowMoving")]
        public IActionResult Index()
        {
            string query = "exec Sp_GetToolSlowMoving";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query);
            return View(dataTable);
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/ToolSlowMoving/ExportExcel")]
        public IActionResult ExportExcel()
        {
            string query = "exec Sp_GetToolSlowMoving";
            DataTable Invoices = new DataProviderLocal().ExecuteQuery(query);

            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForVinamRawSlowMoving(Invoices);


            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
