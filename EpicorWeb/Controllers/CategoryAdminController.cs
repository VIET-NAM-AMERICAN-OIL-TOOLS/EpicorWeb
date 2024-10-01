using EpicorWeb.DAO;
using EpicorWeb.Models;
using Glimpse.Mvc.AlternateType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace EpicorWeb.Controllers
{
    public class CategoryAdminController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private ICompositeViewEngine _viewEngine;

        public CategoryAdminController(IMemoryCache memoryCache, ICompositeViewEngine viewEngine)
        {
            _memoryCache = memoryCache;
            _viewEngine = viewEngine;
        }



        [HttpGet]
        [Route("/CategoryAdminController")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/CategoryAdminController" }).Rows.Count > 0)
                {
                    string query = "exec SP_GetListCategory";
                    DataTable ListCategory = new DataProviderLocal().ExecuteQuery(query);
                    return View(ListCategory);
                }
                else
                {
                    return BadRequest("Bạn không có quyền đăng nhập trang này");
                }
                
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        [HttpGet]
        [Route("/CategoryAdminController/AddCategory")]
        public IActionResult AddCategory(List<int> selectedIDs, List<int> selectedLevels)
        {
            try
            {

                int minLevel = selectedLevels.Count > 0 ? selectedLevels.Min() : 0;
                int flag = 0;
                int selectID = 0;
                string query = "Exec SP_GetLevelFromID @Id";
                // Xử lý mảng ID ở đây
                foreach (int id in selectedIDs)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (int.Parse(new DataProviderLocal().ExecuteQuery(query, new object[] { id }).Rows[0]["level"].ToString()) == minLevel)
                    {
                        flag++;
                        selectID = id;
                    }
#pragma warning restore CS8604 // Possible null reference argument.
                }

                if (flag > 1)
                {
                    return Json(new { success = false, message = "Phát sinh nhiều danh mục cha cùng cấp. Vui lòng chọn lại" });
                }
                else
                {
                    // Tạo đối tượng Category với thông tin cần thiết
                    Category category = new()
                    {
                        ParentID = flag > 0 ? selectID : 0,
                        Level = minLevel + 1
                    };
                    //return PartialView("_addCategoryPartialView", category);
                    PartialViewResult partialView = PartialView("_addCategoryPartialView", category);
                    // Trả về cả hai loại dữ liệu
                    string viewContent = ConvertViewToString(this.ControllerContext, partialView, _viewEngine);
                    return Json(new { viewContent = viewContent, success = true });

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr, ICompositeViewEngine _viewEngine)
        {
            using (StringWriter writer = new StringWriter())
            {
#pragma warning disable CS8604 // Possible null reference argument.
                ViewEngineResult vResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
                ViewContext viewContext = new ViewContext(controllerContext, vResult.View, pvr.ViewData, pvr.TempData, writer, new HtmlHelperOptions());
#pragma warning restore CS8604 // Possible null reference argument.
                vResult.View.RenderAsync(viewContext);
                return writer.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        [Route("/CategoryAdminController/AddCategory")]
        public IActionResult AddCategory(string Name, int ParentID, int Level, string Url)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Url))
            {
                // Trả về một phản hồi lỗi hoặc thực hiện xử lý phù hợp khi thiếu dữ liệu
                return BadRequest("yêu cầu nhập Name và Url.");
            }
            try
            {
                string query = "exec SP_InsertCategory @Name , @ParentId , @Level , @Url";
                int dem = new DataProviderLocal().ExecuteNonQuery(query, new object[] {Name, ParentID, Level,Url});
                if (dem > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest("Thêm mới thất bại!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("/CategoryAdminController/EditCategory")]
        public IActionResult EditCategory(List<int> selectedIDs, List<int> selectedLevels)
        {
            try
            {
                if (selectedIDs.Count == 0) {
                    return BadRequest("Vui lòng chọn 1 danh mục bất kỳ.");
                }

                int minLevel = selectedLevels.Count > 0 ? selectedLevels.Min() : 0;
                int flag = 0;
                int selectID = 0;
                string query = "Exec SP_GetLevelFromID @Id";
                // Xử lý mảng ID ở đây
                foreach (int id in selectedIDs)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (int.Parse(new DataProviderLocal().ExecuteQuery(query, new object[] { id }).Rows[0]["level"].ToString()) == minLevel)
                    {
                        flag++;
                        selectID = id;
                    }
#pragma warning restore CS8604 // Possible null reference argument.
                }

                if (flag > 1)
                {
                    return Json(new { success = false, message = "Chọn nhiều hơn 1 danh mục. Vui lòng chọn lại" });
                }
                else
                {
                    string query1 = "Exec SP_GetCategory @ID";
                    // Tạo đối tượng Category với thông tin cần thiết
                    Category category = new()
                    {
                        ID = selectID,
                        Name = new DataProviderLocal().ExecuteQuery(query1, new object[] { selectID }).Rows[0]["Name"].ToString(),
                        ParentID = int.Parse(new DataProviderLocal().ExecuteQuery(query1, new object[] { selectID }).Rows[0]["ParentID"].ToString()),
                        Level = minLevel,
                        Url = new DataProviderLocal().ExecuteQuery(query1, new object[] { selectID }).Rows[0]["Url"].ToString(),
                    };
                    //return PartialView("_addCategoryPartialView", category);
                    PartialViewResult partialView = PartialView("_editCategoryPartialView", category);
                    // Trả về cả hai loại dữ liệu
                    string viewContent = ConvertViewToString(this.ControllerContext, partialView, _viewEngine);
                    return Json(new { viewContent = viewContent, success = true });

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/CategoryAdminController/EditCategory")]
        public IActionResult EditCategory(int Id, string Name, int ParentId, int Level, string Url)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Url))
            {
                // Trả về một phản hồi lỗi hoặc thực hiện xử lý phù hợp khi thiếu dữ liệu
                return BadRequest("yêu cầu nhập Name và Url.");
            }
            try
            {
                string query = "Exec SP_UpdateCategory @Id , @Name , @ParentId , @Level , @Url";
                int dem = new DataProviderLocal().ExecuteNonQuery(query, new object[] { Id, Name, ParentId, Level, Url });
                if (dem > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest("Cập nhật thất bại!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("/CategoryAdminController/DeleteCategory")]
        public IActionResult DeleteCategory(List<int> selectedIDs, List<int> selectedLevels)
        {
            if (selectedIDs.Count == 0)
            {
                // Trả về một phản hồi lỗi hoặc thực hiện xử lý phù hợp khi thiếu dữ liệu
                return Json(new { success = false, message = "Vui lòng chọn danh mục để xóa!" });
            }
            try
            {
                int minLevel = selectedLevels.Count > 0 ? selectedLevels.Min() : 0;
                int flag = 0;
                int selectID = 0;
                string query = "Exec SP_GetLevelFromID @Id";
                // Xử lý mảng ID ở đây
                foreach (int id in selectedIDs)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (int.Parse(new DataProviderLocal().ExecuteQuery(query, new object[] { id }).Rows[0]["level"].ToString()) == minLevel)
                    {
                        flag++;
                        selectID = id;
                    }
#pragma warning restore CS8604 // Possible null reference argument.
                }

                if (flag > 1)
                {
                    return Json(new { success = false, message = "Chỉ xóa được 1 danh mục tại 1 thời điểm." });
                }
                else
                {
                    string query1 = "exec SP_GetGroupAuthenticatonByMenuId @MenuId";
                    int dem = new DataProviderLocal().ExecuteQuery(query1, new object[] { selectID }).Rows.Count;
                    string query3 = "exec SP_GetChildCategoryByParentID @ParentId";
                    int dem2 = new DataProviderLocal().ExecuteQuery(query3,new object[] { selectID }).Rows.Count;
                    if (dem > 0)
                    {
                        return Json(new { success = false, message = "Danh mục đang được phân quyền không thể xóa." });
                    }
                    else if (dem2 > 0)
                    {
                        return Json(new { success = false, message = "Tồn tại danh mục con không thể xóa." });
                    }
                    else    
                    {
                        string query2 = "exec SP_DeleteCategory @ID";
                        int dem1 = new DataProviderLocal().ExecuteNonQuery(query2, new object[] { selectID });
                        if (dem1 > 0)
                        {
                            return Json(new { success = true, message = "Xóa danh mục thành công." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Xóa danh mục thất bại." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

