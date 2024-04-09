using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Services.Account;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        UserManagement2Context db = new UserManagement2Context();
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;
        public AccountController(ILogger<HomeController> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        // SHOW VIEW
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult NotFound() { return View("404"); }
        // END SHOW VIEW

        [HttpPost]
        public IActionResult CheckLogin(string username, string password)
        {
            List<User> users = db.Users.ToList();

            User user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Check status user
                if (user.Status != "Open")
                {
                    ViewBag.ErrorLogin = "User is locked";
                    return View("Login");
                }

                // User login
                if (user.Priority != "Admin")
                {
                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("IDUserLogin", user.Id.ToString());
                    TempData["FullName"] = HttpContext.Session.GetString("FullName");
                    return RedirectToAction("Home", "User");
                }

                HttpContext.Session.SetString("FullName", user.FullName);
                TempData["FullName"] = HttpContext.Session.GetString("FullName");

                return RedirectToAction("Home", "Admin");
            }

            ViewBag.ErrorLogin = "Incorrect Username or Password!";
            return View("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session?.Clear();
            TempData.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            User user2 = new User();
            user2.FullName = user.FullName;
            user2.Username = user.Username;
            user2.Password = user.Password;
            user2.DateCreated = DateTime.Now;
            user2.DateModified = DateTime.Now;

            var isSave = _accountService.SaveRegisterUser(user2);
            TempData["RegisterDone"] = "Register successfully! Feel free to sign in our page!";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public JsonResult CheckUsername(string username)
        {
            if (username != null)
            {
                var isExist = _accountService.CheckUsername(username);

                return Json(new { response = true, isExist = isExist });

            }
            return Json(new { response = false });
        }
    }
}
