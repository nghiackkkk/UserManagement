using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        UserManagementContext db = new UserManagementContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public static bool IsValidUsername(string username)
        {
            string UsernamePattern = @"^[a-zA-Z0-9_-]{3,16}$";
            return Regex.IsMatch(username, UsernamePattern);
        }

        public static bool IsValidPassword(string password)
        {
            string PasswordPattern = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{3,20}$";
            return Regex.IsMatch(password, PasswordPattern);
        }
        public static bool IsValidNumber(string number)
        {
            string numberPattern = @"^\d{10}$";
            return Regex.IsMatch(number, numberPattern);
        }

        [HttpGet]
        public JsonResult ResetPassword(List<int> ids)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                db.Database.ExecuteSqlRaw($"ResetPassword @id={ids[i]}");
            }

            var users = db.Users.FromSqlRaw("SelectAll").ToList();

            return Json(new
            {
                success1 = true,
                message1 = "Reset passwords for IDs: " + string.Join(", ", ids),
                data = users
            });

            //return "true";
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                ViewBag.Name = HttpContext.Session.GetString("Name");
                List<User> users = new List<User>();
                users = db.Users.Where(u => u.Status.Equals("Open") || u.Status.Equals("Lock")).OrderByDescending(u => u.Id).ToList();
                ViewBag.TotalItem = users.Count;
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
            ViewBag.Name = HttpContext.Session.GetString("Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(string name, int age, string gender,
            string number, string username, string password, string status,
            string priority)
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
                if (user.Username == user1.Username)
                {
                    var error = "Username is already used";
                    ViewBag.UsernameError = error;
                    isError = true;
                }
                if (IsValidPassword(user1.Password) == false)
                {
                    ViewBag.PasswordError = "Password must include uppercase, lowercase, number, 3-10 letter and !@#$%^&*";
                    isError = true;
                }
                if (string.IsNullOrEmpty(user1.Number) || IsValidNumber(user1.Number) == false)
                {
                    ViewBag.NumberError = "Phone number only have number and 10 character";
                    isError = true;
                }
            }

            if (isError)
            {
                return View("ADUser", user1);
            }

            var result = db.Database.ExecuteSqlRaw(
                $"InsertUser @name = '{name}', @age={age}, @gender='{gender}', " +
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
            ViewBag.Name = HttpContext.Session.GetString("Name");
            HttpContext.Session.SetString("EditId", id.ToString());
            User user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUserDo(string name, int age, string gender,
            string number, string username, string password, string status,
            string priority, int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Name");
            List<User> users = new List<User>();
            users = db.Users.ToList();
            User user1 = new User();
            User user2 = new User();
            var idEdit = Int32.Parse(HttpContext.Session.GetString("EditId"));
            user2 = db.Users.Find(idEdit);

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
                    if (user.Username == user1.Username)
                    {
                        var error = "Username is already used";
                        ViewBag.UsernameError = error;
                        isError = true;
                    }
                    if (IsValidPassword(user1.Password) == false)
                    {
                        ViewBag.PasswordError = "Password must include uppercase, lowercase, number, 3-10 letter and !@#$%^&*";
                        isError = true;
                    }
                    if (string.IsNullOrEmpty(user1.Number) || IsValidNumber(user1.Number) == false)
                    {
                        ViewBag.NumberError = "Phone number only have 10 digit character";
                        isError = true;
                    }
                }
            }

            if (isError)
            {
                return View("EditUser", user1);
            }

            //var result = db.Database.ExecuteSqlRaw(
            //    $"EXEC dbo.UpdateUser @id={idEdit}, @name = '{name}', @age={age}, @gender='{gender}', " +
            //    $"@status='{status}', @number='{number}', @username='{username}', " +
            //    $"@password='{password}', @priority='{priority}'");

            var res = db.Database.ExecuteSqlRaw(
                $"UpdateUser @id={idEdit}, @name = '{name}', @age={age}, @gender='{gender}', @status='{status}', @number='{number}', @username='{username}', @password='{password}', @priority='{priority}'");

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public JsonResult Search(string input)
        {
            var result = db.Users.Where(
                u => u.Username.Contains(input)
                || u.Name.Contains(input)
                || u.Number.Contains(input)
                || u.Password.Contains(input)
            ).ToList();

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public JsonResult GetAllItem(string sort)
        {
            switch (sort)
            {
                case "none":
                    var result = db.Users.Where(u => u.Status.Equals("Open") || u.Status.Equals("Lock")).OrderByDescending(u => u.Id).ToList();
                    return Json(result);

                case "nameaz":
                    result = db.Users.Where(
                    u => u.Status.Equals("Open") ||
                    u.Status.Equals("Lock")).OrderBy(u => u.Name).ToList();
                    return Json(result);

                case "nameza":
                    result = db.Users.Where(
                    u => u.Status.Equals("Open") ||
                    u.Status.Equals("Lock")).OrderByDescending(u => u.Name).ToList();
                    return Json(result);

                case "usernameaz":
                    result = db.Users.Where(
                    u => u.Status.Equals("Open") ||
                    u.Status.Equals("Lock")).OrderBy(u => u.Username).ToList();
                    return Json(result);

                case "usernameza":
                    result = db.Users.Where(
                    u => u.Status.Equals("Open") ||
                    u.Status.Equals("Lock")).OrderByDescending(u => u.Username).ToList();
                    return Json(result);

                default:
                    result = db.Users.Where(u => u.Status.Equals("Open") || u.Status.Equals("Lock")).OrderByDescending(u => u.Id).ToList();
                    return Json(result);

            }

            if (sort.Equals("none"))
            {
                var result = db.Users.Where(u => u.Status.Equals("Open") || u.Status.Equals("Lock")).OrderByDescending(u => u.Id).ToList();
                return Json(result);
            }
            else if (sort.Equals("nameaz"))
            {
                var result = db.Users.Where(
                    u => u.Status.Equals("Open") ||
                    u.Status.Equals("Lock")).OrderBy(u => u.Name).ToList();
                return Json(result);
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public JsonResult UpdateStatus(int id)
        {
            var result = db.Database.ExecuteSql($"UpdateStatusUser {id}");
            //var users = db.Users.Where(x => x.Status.) FromSqlRaw("SelectAll").ToList();

            var users = db.Users.Where(u => u.Status.Equals("Open") || u.Status.Equals("Lock")).OrderByDescending(u => u.Id);
            return Json(new { success = true, data = users });
        }

        [HttpGet]
        public JsonResult DeleteUserDo(int id)
        {
            var result = db.Database.ExecuteSqlRaw(
                $"DeleteUser" +
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
                if (user.Username == username && user.Password == password)
                {
                    if (user.Status != "Open")
                    {
                        ViewBag.ErrorMes = "User is locked";
                        return View("Login");
                    }
                    //if (user.Priority != "admin")
                    //{
                    //    ViewBag.ErrorMes = "Only admin can access";
                    //    return View("Login");
                    //}
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

        [HttpGet]
        public IActionResult ShowTrash()
        {
            var usersTrash = db.Users.Where(u => u.Status.Equals("OnTrash")).OrderByDescending(u => u.Id).ToList();
            ViewBag.TotalItem = usersTrash.Count();
            ViewBag.Name = HttpContext.Session.GetString("Name");
            return View(usersTrash);
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

        [HttpGet]
        public JsonResult MoveToTrash(List<int> ids)
        {
            // Set status to OnTrash
            for (int i = 0; i < ids.Count; i++)
            {
                db.Database.ExecuteSqlRaw($"MoveToTrash {ids[i]}");
            }

            // Return list item dont have status OnTrash
            var result = db.Users.Where(u => u.Status.Equals("Open") || u.Status.Equals("Lock")).OrderByDescending(u => u.Id);

            return Json(result);

        }

        [HttpGet]
        public JsonResult RestoreUser(List<int> ids)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                db.Database.ExecuteSqlRaw($"RestoreTrash {ids[i]}");
            }
            var usersTrash = db.Users.Where(u => u.Status.Equals("OnTrash")).OrderByDescending(u => u.Id).ToList();
            return Json(new { success = true, data = ids });
        }

        [HttpGet]
        public JsonResult GetAllItemTrash()
        {
            var usersTrash = db.Users.Where(u => u.Status.Equals("OnTrash")).OrderByDescending(u => u.Id).ToList();
            return Json(usersTrash);
        }

        [HttpGet]
        public JsonResult SearchDelete(string input)
        {
            var result = db.Users.Where(
                u => (u.Username.Contains(input)
                || u.Name.Contains(input)
                || u.Number.Contains(input)
                || u.Password.Contains(input))
                && u.Status.EndsWith("OnTrash")
            ).ToList();

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
