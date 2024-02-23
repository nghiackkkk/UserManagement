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
        private static readonly string PasswordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{3,20}$";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public static bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, UsernamePattern);
        }

        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, PasswordPattern);
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
            return Redirect("/");
        }
        public IActionResult ADUser()
        {
            if (HttpContext.Session.GetString("Name") == null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(string name, int age, string gender, string number, string username, string password, string status, string priority)
        {

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

            bool isError = false;

            // Validate username & password
            foreach (var user in users)
            {
                if (user.Username == username)
                {
                    var error = "Username is already used";
                    ViewBag.UsernameError = error;
                    isError = true;

                }
                if (IsValidPassword(password) == false)
                {
                    ViewBag.PasswordError = "Password must include uppercase, lowercase, number, 3-10 letter and symbol";
                    isError = true;
                }
            }

            if (isError)
            {
                return View("ADUser", user1);
            }

            var result = db.Database.ExecuteSqlRaw(
                $"EXEC dbo.InsertUser @name = '{name}', @age={age}, @gender='{gender}', " +
                $"@status='{status}', @number='{number}', @username='{username}', " +
                $"@password='{password}', @priority='{priority}'");

            return Redirect("/Home/Index");
        }


        public IActionResult EditUser(int id)
        {
            if (HttpContext.Session.GetString("Name") == null)
            {
                return Redirect("/");
            }
            User user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUserDo(string name, int age, string gender,
            string number, string username, string password, string status,
            string priority, int id)
        {
            List<User> users = new List<User>();
            users = db.Users.ToList();
            User user1 = new User();
            User user2 = new User();
            user2 = db.Users.Find(id);

            user1.Username = username;
            user1.Age = age;
            user1.Gender = gender;
            user1.Number = number;
            user1.Priority = priority;
            user1.Name = name;
            user1.Password = password;
            user1.Status = status;

            bool isError = false;

            // Validate username & password
            foreach (var user in users)
            {
                if (user2.Username == user.Username)
                {
                    continue;
                }
                else
                {
                    if (user.Username == username)
                    {
                        var error = "Username is already used";
                        ViewBag.UsernameError = error;
                        isError = true;

                    }
                    if (IsValidPassword(password) == false)
                    {
                        ViewBag.PasswordError = "Password must include uppercase, lowercase, number, 3-10 letter and symbol";
                        isError = true;
                    }
                }
            }

            if (isError)
            {
                return View("EditUser", user1);
            }

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
