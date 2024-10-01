using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class EFFReportController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public EFFReportController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/EFFReport")]
        public IActionResult Index()
        {
            string query = "exec SP_GetEFF";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query);
            return View(dataTable);
        }
    }
}
