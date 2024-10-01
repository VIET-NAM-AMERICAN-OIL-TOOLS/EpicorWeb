using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace EpicorWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public UsersController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("/UsersController")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string queryUrl = "Exec SP_CheckUrlByUser @UserId , @Url";
                if (new DataProviderLocal().ExecuteQuery(queryUrl, new object[] { HttpContext.Session.GetString("user").Trim() , "/UsersController" }).Rows.Count > 0)
                {
                    string query = "exec SP_GetListUser";
                    DataTable VinamListUsers = new DataProviderLocal().ExecuteQuery(query);
                    List<Users> UserList = new();
                    foreach (DataRow ListUsers in VinamListUsers.Rows)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        Users users = new()
                        {
                            ID = ListUsers["ID"].ToString(),
                            Name = ListUsers["UserName"].ToString(),
                            GroupName = ListUsers["GroupName"].ToString(),
                            GroupID = int.Parse(ListUsers["GroupID"].ToString())
                        };
#pragma warning restore CS8604 // Possible null reference argument.

                        UserList.Add(users);
                    }
                    return View(UserList);
                }
                else
                {
                    return BadRequest("Bạn không có quyền đăng nhập trang này");
                }    
                
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        [HttpPost]
        [Route("/UsersController/UpdateUsers")]
        public int UpdateUsers()
        {
            int dem = 0;

            string query = "exec SP_InsertUserFromEpicorUser";
            dem = new DataProviderLocal().ExecuteNonQuery(query);

            return dem;
        }

        [HttpGet]
        [Route("/UsersController/EditUsers")]
        public IActionResult EditUsers(string Id, string Name, string GroupName, int GroupID)
        {
            Users user = new Users();
            user.ID = Id;
            user.Name = Name;
            user.GroupName = GroupName;
            user.GroupID = GroupID;

            string query = "Exec SP_GetGroupAll";
            var ListMenuID = new DataProviderLocal().ExecuteQuery(query);

            // Chuyển đổi DataTable thành danh sách ID
            var menuIds = ListMenuID.AsEnumerable()
                                 .Select(row => new Groups
                                 {
                                     ID = Convert.ToInt32(row["ID"]),
                                     Name = Convert.ToString(row["Name"]),
                                 })
                                 .AsEnumerable();

            // Gán danh sách ID vào ViewBag
            ViewBag.GroupAll = menuIds;

            return PartialView("_editUsersPartialView", user);
        }

        [HttpPost]
        [Route("/UsersController/EditUsers")]
        public IActionResult EditUsers(string Id, int GroupID)
        {
            string query = "Exec SP_InsertUserCategoryAuthetication @UserId , @GroupName";
            int dem = new DataProviderLocal().ExecuteNonQuery (query, new object[] {Id, GroupID });
            return RedirectToAction("Index");
        }
    }
}
