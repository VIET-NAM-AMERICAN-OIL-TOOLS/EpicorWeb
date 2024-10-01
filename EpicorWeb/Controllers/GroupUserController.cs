using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Text.RegularExpressions;

namespace EpicorWeb.Controllers
{
    public class GroupUserController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public GroupUserController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/GroupUserController")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim(), "/GroupUserController" }).Rows.Count > 0)
                {
                    string query = "exec SP_GetListGroups";
                    DataTable VinamListUsers = new DataProviderLocal().ExecuteQuery(query);
                    List<Groups> GroupList = new();
                    foreach (DataRow ListGroups in VinamListUsers.Rows)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        Groups groups = new()
                        {
                            ID = int.Parse(ListGroups["ID"].ToString()),
                            Name = ListGroups["Name"].ToString(),
                            Description = ListGroups["Description"].ToString()
                        };
#pragma warning restore CS8604 // Possible null reference argument.

                        GroupList.Add(groups);
                    }
                    return View(GroupList);
                }
                else
                {
                    return BadRequest("Bạn không có quyền đăng nhập trang này");
                }
                
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        [HttpGet]
        [Route("/GroupUserController/CreateGroup")]
        public IActionResult CreateGroup() {
            Groups groups = new Groups();
            return PartialView("_addGroupPartialView", groups);
        }

        [HttpPost]
        [Route("/GroupUserController/CreateGroup")]
        public IActionResult CreateGroup(string Name, string Description)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description))
            {
                // Trả về một phản hồi lỗi hoặc thực hiện xử lý phù hợp khi thiếu dữ liệu
                return BadRequest("yêu cầu nhập Name và Description.");
            }
            string query = "exec SP_InsertGroups @Name , @Description";
            int result = new DataProviderLocal().ExecuteNonQuery(query, new object[] { Name, Description });
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/GroupUserController/EditGroup")]
        public IActionResult EditGroup(int ID ,string Name, string Description)
        {
            Groups groups = new Groups();
            groups.ID = ID;
            groups.Name = Name;
            groups.Description = Description;
            return PartialView("_editGroupPartialView", groups);
        }

        [HttpPost]
        [Route("/GroupUserController/EditGroup")]
        public IActionResult EditsGroup(int ID, string Name, string Description)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description))
            {
                // Trả về một phản hồi lỗi hoặc thực hiện xử lý phù hợp khi thiếu dữ liệu
                return BadRequest("yêu cầu nhập Name và Description.");
            }
            string query = "exec SP_UpdateGroups @ID , @Name , @Description";
            int result = new DataProviderLocal().ExecuteNonQuery(query, new object[] { ID, Name, Description });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/GroupUserController/DeleteGroup")]
        public JsonResult DeleteGroup(int id)
        {
            try
            {
                string query = "exec SP_CountRowFollowGroup @GroupID";
                // Xóa nhóm dựa trên tên nhóm
                if(new DataProviderLocal().ExecuteQuery(query, new object[] { id }).Rows.Count == 0)
                {
                    string query1 = "exec SP_DeleteGroup @GroupID";
                    new DataProviderLocal().ExecuteQuery(query1, new object[] { id });
                    return Json(new { success = true, message = "Nhóm đã được xóa thành công." });
                }
                else
                {
                    return Json(new { success = false, message = "Nhóm đã được sử dụng không thể xóa." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("/GroupUserController/DecentralizationGroup")]
        public IActionResult DecentralizationGroup(int ID)
        {
            string query = "exec SP_GetListCategory";
            DataTable ListCategory = new DataProviderLocal().ExecuteQuery(query);

            string query1 = "exec [dbo].[SP_GetGroupAuthenticatonByGroupId] @GroupId";
            var ListMenuID = new DataProviderLocal().ExecuteQuery(query1, new object[] { ID });

            // Chuyển đổi DataTable thành danh sách ID
            List<int> menuIds = ListMenuID.AsEnumerable().Select(row => Convert.ToInt32(row["MenuID"])).ToList();

            // Gán danh sách ID vào ViewBag
            ViewBag.CheckedMenuIds = menuIds;
            ViewBag.GroupID = ID;
            return PartialView("_decentralizationPartialView", ListCategory);
        }

        [HttpPost]
        [Route("/GroupUserController/DecentralizationGroup")]
        public IActionResult DecentralizationGroup(List<int> selectedIDs, int GroupID)
        {
            try
            {
                string query = "exec SP_DeleteGroupAuthentication @GroupId";
                string query1 = "Exec SP_InsertGroupAuthentication @MenuId , @GroupId";
                if (selectedIDs.Count > 0 && GroupID > 0)
                {
                    new DataProviderLocal().ExecuteNonQuery(query, new object[] {GroupID});
                    foreach (int id in selectedIDs)
                    {
                        new DataProviderLocal().ExecuteNonQuery(query1, new object[] {id,GroupID});
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest("Quá trình phân quyền đã xảy ra lỗi.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
