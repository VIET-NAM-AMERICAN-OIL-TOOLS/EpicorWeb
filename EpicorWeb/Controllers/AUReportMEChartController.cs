using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class AUReportMEChartController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public AUReportMEChartController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/AUReportMEChart")]
        public IActionResult Index()
        {
            string query = "exec SP_AUBySalesMonth";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query);

            return View(dataTable);
        }
    }
}
