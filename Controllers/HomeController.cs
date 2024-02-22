using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        UserManagementContext db = new UserManagementContext();

        private readonly ILogger<HomeController> _logger;
        private static readonly string UsernamePattern = @"^[a-zA-Z0-9_-]{3,16}$";
        private static readonly string PasswordPattern = @"^[a-zA-Z0-9!@#$%^&*]{3,10}$";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                ViewBag.Name = HttpContext.Session.GetString("Name");
                List<User> users = new List<User>();
                users = db.Users.ToList();

                return View(users);
            }
            return Redirect("/Home/Login");
        }
        public IActionResult ADUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(string name, int age, string gender, string number, string username, string password, string status, string priority)
        {
            // Check duplicate username
            List<User> users = new List<User>();
            users = db.Users.ToList();
            User user1 = new User();

            user1.Username = username;
            user1.Age = age;
            user1.Gender = gender;
            user1.Number = number;
            user1.Priority = priority;
            user1.Name = name;
            user1.Password = password;
            user1.Status = status;
            foreach (var user in users)
            {
                if (user.Username == username) {
                    var error = "Username is already used";
                    ViewBag.UsernameError = error;
                    return View("ADUser", user1);
                } else if (IsValidPassword(password)) {
                    ViewBag.PasswordError = "Password must include uppercase, lowercase, number, 3-10 letter and symbol";
                    return View("ADUser", user1);
                }
            }

            // Check password required Uppercase, Lowercase, Number, letter 3-10, inlcude ~!@#$%^&*
            


            var result = db.Database.ExecuteSqlRaw(
                $"EXEC dbo.InsertUser @name = '{name}', @age={age}, @gender='{gender}', " +
                $"@status='{status}', @number='{number}', @username='{username}', " +
                $"@password='{password}', @priority='{priority}'");

            return Redirect("/Home/Index");
        }


        public IActionResult EditUser(int id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUserDo(string name, int age, string gender,
            string number, string username, string password, string status,
            string priority, int id)
        {
            var result = db.Database.ExecuteSqlRaw(
                $"EXEC dbo.UpdateUser @id={id}, @name = '{name}', @age={age}, @gender='{gender}', " +
                $"@status='{status}', @number='{number}', @username='{username}', " +
                $"@password='{password}', @priority='{priority}'");
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public JsonResult UpdateStatus(int id)
        {
            var result = db.Database.ExecuteSqlRaw(
                $"EXEC dbo.UpdateStatusUser " +
                $"@id = {id}");

            return Json(new { success = true }); ;
        }

        [HttpGet]
        public JsonResult DeleteUserDo(int id)
        {
            var result = db.Database.ExecuteSqlRaw(
                $"EXEC dbo.DeleteUser" +
                $" @id = {id}");
            return Json(new { success = true });
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public static bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, UsernamePattern);
        }

        // Validate password
        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, PasswordPattern);
        }

        [HttpPost]
        public IActionResult CheckLogin(String username, String password)
        {
            var error = "";

            List<User> users = new List<User>();
            users = db.Users.ToList();

            foreach (User user in users)
            {
                if (user.Username == username && user.Password == password
                    && user.Status == "Open")
                {
                    HttpContext.Session.SetString("Name", user.Name);
                    return Redirect("/Home/Index");
                }
            }
            error = "Incorrect Username or Password!";
            ViewBag.ErrorMes = error;
            return View("Login");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Name");
            return Redirect("Login");
        }

        public String Test()
        {
            List<User> users = new List<User>();
            users = db.Users.ToList();
            return users[0].Username;
        }
        public IActionResult Privacy()
        {
            return View();
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
