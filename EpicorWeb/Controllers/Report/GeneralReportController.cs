using DocumentFormat.OpenXml.Spreadsheet;
using EpicorWeb.Common;
using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Drawing.Printing;

namespace EpicorWeb.Controllers.Report
{
	public class GeneralReportController : Controller
	{
		private readonly IMemoryCache _memoryCache;
		public List<VinamPartOnHandQuantity> ListPartOnHandQuantity = new List<VinamPartOnHandQuantity>();
		public List<VinamPartOnHandConsumables> ListPartOnHandConsumables = new List<VinamPartOnHandConsumables>();
		public GeneralReportController(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		[HttpGet]
		[Route("/GeneralReport/VinamPartList")]
		public IActionResult VinamPartList()
		{
			if (HttpContext.Session.GetString("user") != null)
			{
				string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
				if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/GeneralReport/VinamPartList" }).Rows.Count > 0)
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

		[HttpGet]
		[Route("/GeneralReport/VinamLaborHourTransaction")]
		public IActionResult VinamLaborHourTransaction()
		{
			if (HttpContext.Session.GetString("user") != null)
			{
				string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
				if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/GeneralReport/VinamLaborHourTransaction" }).Rows.Count > 0)
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
	
		[HttpGet]
		[Route("/GeneralReport/VinamPartOnHand")]
		public IActionResult VinamPartOnHand()
		{
			string query = "exec SPD_GeneralReport_VinamPartOnHandQuantity";
			DataTable data = new DataProviderLocal().ExecuteQuery(query);
			ListPartOnHandQuantity = Commonsetup.ConvertDataTableToObject<VinamPartOnHandQuantity>(data);
			return View( ListPartOnHandQuantity);

		}

		[HttpGet]
		[Route("/GeneralReport/VinamPartOnHandConsumables")]
		public IActionResult VinamPartOnHandConsumables()
		{
			string query = "exec SPD_GeneralReport_VinamPartOnHandConsumables";
			DataTable data = new DataProviderLocal().ExecuteQuery(query);
			ListPartOnHandConsumables = Commonsetup.ConvertDataTableToObject<VinamPartOnHandConsumables>(data);
			return View(ListPartOnHandConsumables);
		}

		[HttpGet]
		[Route("/GeneralReport/VinamJobTracker")]
		public IActionResult VinamJobTracker()
		{
			string query = "exec SPD_GeneralReport_VinamJobTracker";
			DataTable data = new DataProviderLocal().ExecuteQuery(query);
			var VinamJobTracker = Commonsetup.ConvertDataTableToObject<VinamJobTracker>(data);
			return View(VinamJobTracker);
		}

		[HttpPost]
		[Route("/GeneralReport/ExportVinamPartList")]
		public IActionResult ExportVinamPartList()
		{
			string query = "exec SPD_GeneralReport_VinamPartList";
			DataTable data = new DataProviderLocal().ExecuteQuery(query);

			var fileContents = Commonsetup.ExportDataTableToExcel(data, "VinamPartList");


			// Trả về tệp tin Excel
			return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
		}

		[HttpPost]
		[Route("/GeneralReport/ExportVinamLaborHourTransaction")]
		public IActionResult ExportVinamLaborHourTransaction(DateTime fromDate, DateTime toDate)
		{
			try
			{
				string query = "exec SPD_GeneralReport_VinamLaborHourTransaction @FromDate , @ToDate";
				DataTable data = new DataProviderLocal().ExecuteQuery(query, new object[] { fromDate, toDate });
				var fileContents = Commonsetup.ExportDataTableToExcel(data, "VinamLaborHourTransaction");

				return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
			}
			catch (Exception e)
			{
				return null;
			}

		}

		[HttpPost]
		[Route("/GeneralReport/ExportVinamPartOnHandQuantity")]
		public IActionResult ExportVinamPartOnHandQuantity()
		{
			try
			{
				string query = "exec SPD_GeneralReport_VinamPartOnHandQuantity";
				DataTable data = new DataProviderLocal().ExecuteQuery(query);
				var fileContents = Commonsetup.ExportDataTableToExcel(data, "VinamPartOnHandQuantity");

				return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
			}
			catch (Exception e)
			{
				return null;
			}

		}

		[HttpPost]
		[Route("/GeneralReport/ExportVinamPartOnHandConsumables")]
		public IActionResult ExportVinamPartOnHandConsumables()
		{
			try
			{
				string query = "exec SPD_GeneralReport_VinamPartOnHandConsumables";
				DataTable data = new DataProviderLocal().ExecuteQuery(query);
				var fileContents = Commonsetup.ExportDataTableToExcel(data, "VinamPartOnHandConsumables");

				return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
			}
			catch (Exception e)
			{
				return null;
			}

		}

		[HttpPost]
		[Route("/GeneralReport/ExportVinamJobTracker")]
		public IActionResult ExportVinamJobTracker()
		{
			string query = "exec SPD_GeneralReport_VinamJobTracker";
			DataTable data = new DataProviderLocal().ExecuteQuery(query);

			var fileContents = Commonsetup.ExportDataTableToExcel(data, "VinamJobTracker");
			// Trả về tệp tin Excel
			return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
		}

	}
}
