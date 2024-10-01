using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class AUReportController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public AUReportController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/AUReport")]
        public IActionResult Index()
        {
            string query = "exec SP_GetAU";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query);

            string query1 = "exec SP_GetTotalTableAU";
            DataTable dataTable1 = new DataProviderLocal().ExecuteQuery(query1);
            decimal au = Math.Round(((decimal.Parse(dataTable1.Rows[0]["P"].ToString()) + decimal.Parse(dataTable1.Rows[0]["S"].ToString())) 
                / decimal.Parse(dataTable1.Rows[0]["GrandTotal"].ToString()) * 100),0);
            ViewBag.au = au;
            return View(dataTable);
        }
    }
}
