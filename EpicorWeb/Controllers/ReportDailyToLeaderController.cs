using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Globalization;

namespace EpicorWeb.Controllers
{
    public class ReportDailyToLeaderController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ReportDailyToLeaderController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/ReportDailyToLeaderController")]
        public IActionResult Index()
        {
            string query = "exec SP_DailyReportEFF";
            DataTable dataTable = new DataProviderLocal().ExecuteQuery(query);
            foreach (DataRow row in dataTable.Rows)
            {
                decimal laborHrs;
                decimal LaborQty;
                decimal ScrapQty;
                decimal RunQty;
                decimal LaborEFF;
                if (decimal.TryParse(row["LaborDtl_LaborHrs"].ToString(), out laborHrs))
                {
                    row["LaborDtl_LaborHrs"] = laborHrs.ToString("0.0#####");
                }
                if (decimal.TryParse(row["LaborDtl_LaborQty"].ToString(), out LaborQty))
                {
                    row["LaborDtl_LaborQty"] = LaborQty.ToString("0.0#####");
                }
                if (decimal.TryParse(row["LaborDtl_ScrapQty"].ToString(), out ScrapQty))
                {
                    row["LaborDtl_ScrapQty"] = ScrapQty.ToString("0.0#####");
                }
                if (decimal.TryParse(row["JobOper_RunQty"].ToString(), out RunQty))
                {
                    row["JobOper_RunQty"] = RunQty.ToString("0.0#####");
                }
                if (decimal.TryParse(row["Calculated_LaborEFF"].ToString(), out LaborEFF))
                {
                    row["Calculated_LaborEFF"] = LaborEFF.ToString("0.0#####");
                }
            }
            return View(dataTable);
        }
    }
}
