using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class AUReportMEController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public AUReportMEController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/AUReportME")]
        public IActionResult Index()
        {

            string query = "exec SP_AUBySalesMonth";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query);

            string query1 = "exec SP_AUBySalesDays";
            DataTable dataTable1 = new DataProviderLocal().ExecuteQuery(query1);

            string query2 = "exec SP_AUByLinesSalesDay";
            DataTable dataTable2 = new DataProviderLocal().ExecuteQuery(query2);

            string query3 = "exec SP_AUByLinesSalesMonth";
            DataTable dataTable3 = new DataProviderLocal().ExecuteQuery(query3);

            ViewBag.AUBySalesDays = dataTable1;
            ViewBag.AUByLinesSalesDay = dataTable2;
            ViewBag.AUByLinesSalesMonth = dataTable3;

            return View(dataTable);
        }
    }
}
