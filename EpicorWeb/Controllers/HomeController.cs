using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EpicorWeb.DAO;
using System.Data;
using System.Web;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace EpicorWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("~/")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                return View();
            }else { return RedirectToAction("Login"); }
            
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string user, string passWord)
        {
            
            if (new LoginDAO().checkUsername(user) > 0)
            {
                if (new DecrpytPass().VerifyHash(passWord, new LoginDAO().getPassFromUserUsername(user)))
                {
                    HttpContext.Session.SetString("user", user);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Mật khẩu của tài khoản " + user + " không đúng!";
                    return View();
                }
            }
            else
            {
                TempData["Error"] = "Tài khoản không tồn tại!";
                return View();
            }
            
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // xóa session
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}