
﻿using EpicorWeb.DAO;
using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Mvc;

namespace EpicorWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            string User = HttpContext.Session.GetString("user");
            string query = "Exec SP_GetCategoryListByUserId @UserId";
            var listCategory = new DataProviderLocal().ExecuteQuery(query, new object[] { User });
            ViewBag.User = User;
            return PartialView(listCategory);
        }
    }
}
