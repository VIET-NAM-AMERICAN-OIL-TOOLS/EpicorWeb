using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class VNIssueMisMaterialController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public VNIssueMisMaterialController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/VNIssueMisMaterial")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/VNIssueMisMaterial" }).Rows.Count > 0)
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

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/VNIssueMisMaterial/VNIssueMisMaterialExportExcel")]
        public IActionResult VNIssueMisMaterialExportExcel(DateTime startDate, DateTime endDate)
        {
            string query = "exec VNSP_GetIssueMiscMtl @Fromdate , @Todate";
            DataTable Invoices = new DataProvider().ExecuteQuery(query, new object[] { startDate, endDate });
            List<IssueMiscMtl> issueMiscMtls = new();
            foreach (DataRow invoice in Invoices.Rows)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                IssueMiscMtl inv = new()
                {
                    Company = invoice["PartTran_Company"].ToString(),
                    TranDate = DateTime.Parse(invoice["PartTran_TranDate"].ToString()).Date,
                    TranType = invoice["PartTran_TranType"].ToString(),
                    PartNum = invoice["PartTran_PartNum"].ToString(),
                    PartDescription = invoice["PartTran_PartDescription"].ToString(),
                    ClassID = invoice["Part_ClassID"].ToString(),
                    GrossWeight = decimal.Parse(invoice["Part_GrossWeight"].ToString()),
                    GrossWeightUOM = invoice["Part_GrossWeightUOM"].ToString(),
                    Description = invoice["Warehse_Description"].ToString(),
                    BinNum = invoice["PartTran_BinNum"].ToString(),
                    Name = invoice["Plant_Name"].ToString(),
                    LotNum = invoice["PartTran_LotNum"].ToString(),
                    Batch = invoice["PartLot_Batch"].ToString(),
                    MfgBatch = invoice["PartLot_MfgBatch"].ToString(),
                    LegalNumber = invoice["PartTran_LegalNumber"].ToString(),
                    TranQty = decimal.Parse(invoice["PartTran_TranQty"].ToString()),
                    UM = invoice["PartTran_UM"].ToString(),
                    ExtCost = decimal.Parse(invoice["PartTran_ExtCost"].ToString()),
                    EntryPerson = invoice["PartTran_EntryPerson"].ToString(),
                    TranNum = int.Parse(invoice["PartTran_TranNum"].ToString()),
                    CalTranNum = invoice["Calculated_TranNum"].ToString(),
                    FilterTransaction = invoice["Calculated_FilterTransaction"].ToString(),
                    Key1 = invoice["UD09_Key1"].ToString(),
                    Reference = invoice["Calculated_Reference"].ToString(),
                    Character03 = invoice["UD09_Character03"].ToString()
                };
#pragma warning restore CS8604 // Possible null reference argument.

                issueMiscMtls.Add(inv);
            }
            byte[] fileContents = new ExportExcelWithEpplus().ExportExcelWithEpplusForIssueMiscMtl(issueMiscMtls);

            //// Lưu workbook vào MemoryStream
            //var stream = new MemoryStream(package.GetAsByteArray());

            // Thiết lập tên file
            string fileName = "IssueMiscMtls.xlsx";

            // Trả về tệp tin Excel
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
