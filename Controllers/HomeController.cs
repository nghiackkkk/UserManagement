using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Identity.Client;
using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UserManagement.Models;
using SelectPdf;
using static System.Runtime.InteropServices.JavaScript.JSType;
using iText.Html2pdf;
using System;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        UserManagementContext db = new UserManagementContext();
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            hostingEnvironment = environment;
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

        static string RemoveVietnameseDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            Regex regex = new Regex("\\p{M}", RegexOptions.Compiled);

            string result = regex.Replace(normalizedString, string.Empty);
            return result;
        }

        private bool RemoveUnusedImage()
        {
            string directPath = "wwwroot\\uploads";
            List<string> imageNames = GetImageName(directPath);
            List<User> users = db.Users.ToList();

            foreach (var name in imageNames)
            {
                bool check = false;
                foreach (var user in users)
                {

                    if (user.Image != null)
                    {
                        if (user.Image == name)
                        {
                            check = true;
                        }
                    }
                }
                if (check == false)
                {
                    string imagePath = Path.Combine(directPath, name);
                    System.IO.File.Delete(imagePath);
                }
            }

            return true;
        }

        private List<string> GetImageName(string directoryPath)
        {
            List<string> imageNames = new List<string>();

            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file).ToLower();
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {

                        imageNames.Add(Path.GetFileName(file));
                    }
                }
            }
            else
            {
                Console.WriteLine("No directory");
            }

            return imageNames;
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
                RemoveUnusedImage();
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
        public IActionResult CreateUser(UserViewsModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid model data");
            }
            if (model.Name == null || model.Username == null || model.Password == null)
            {
                return BadRequest("Name, Username, and Password are required fields");
            }

            if (model.Photo == null)
            {
                return BadRequest("Photo is null");
            }

            List<User> users = new List<User>();
            users = db.Users.ToList();
            User user1 = new User();

            user1.Name = model.Name;
            user1.Username = model.Username;
            user1.Password = model.Password;
            user1.Status = model.Status;
            user1.Number = model.Number;
            user1.Gender = model.Gender;
            user1.Age = model.Age;
            user1.Priority = model.Priority;

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

            var uniqueFileName = GetUniqueFileName(model.Photo.FileName);
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

            user1.Image = uniqueFileName;

            if (isError)
            {
                return View("ADUser", model);
            }

            var result = db.Database.ExecuteSqlRaw(
                $"CreateUser @name = '{model.Name}', @age={model.Age}, @gender='{model.Gender}', " +
                $"@status='{model.Status}', @number='{model.Number}', @username='{model.Username}', " +
                $"@password='{model.Password}', @priority='{model.Priority}'," +
                $"@image='{uniqueFileName}'");

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
            UserViewsModel userViewModel = new UserViewsModel();
            userViewModel.Id = id;
            userViewModel.Name = user.Name;
            userViewModel.Age = user.Age;
            userViewModel.Number = user.Number;
            userViewModel.Status = user.Status;
            userViewModel.Priority = user.Priority;
            userViewModel.Gender = user.Gender;
            userViewModel.Username = user.Username;
            userViewModel.Password = user.Password;
            userViewModel.Photo = null;

            ViewBag.UserPhoto = user.Image;

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult EditUserDo(UserViewsModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid model data");
            }
            if (model.Name == null || model.Username == null || model.Password == null)
            {
                return BadRequest("Name, Username, and Password are required fields");
            }



            ViewBag.Username = HttpContext.Session.GetString("Name");
            List<User> users = new List<User>();
            users = db.Users.ToList();
            User user1 = new User();
            User user2 = new User();
            var idEdit = Int32.Parse(HttpContext.Session.GetString("EditId"));
            user2 = db.Users.Find(idEdit);

            user1.Name = model.Name;
            user1.Username = model.Username;
            user1.Password = model.Password;
            user1.Status = model.Status;
            user1.Number = model.Number;
            user1.Gender = model.Gender;
            user1.Age = model.Age;
            user1.Priority = model.Priority;

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

                return View("EditUser", model);
            }

            if (model.Photo == null)
            {
                db.Database.ExecuteSqlRaw(
                $"UpdateUserNoImage @id={idEdit}, @name = '{model.Name}', @age={model.Age}," +
                $" @gender='{model.Gender}', @status='{model.Status}', @number='{model.Number}'," +
                $" @username='{model.Username}', @password='{model.Password}'," +
                $" @priority='{model.Priority}'");

                return Redirect("Index");
            }

            var uniqueFileName = GetUniqueFileName(model.Photo.FileName);
            uniqueFileName = RemoveVietnameseDiacritics(uniqueFileName);
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

            user1.Image = uniqueFileName;

            db.Database.ExecuteSqlRaw(
            $"UpdateUser @id={idEdit}, @name = '{model.Name}', @age={model.Age}," +
            $" @gender='{model.Gender}', @status='{model.Status}', @number='{model.Number}'," +
            $" @username='{model.Username}', @password='{model.Password}'," +
            $" @priority='{model.Priority}', @image='{uniqueFileName}'");

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public JsonResult Search(string input)
        {
            var result = db.Users.Where(
                u => (u.Username.Contains(input)
                || u.Name.Contains(input)
                || u.Number.Contains(input)
                || u.Password.Contains(input))
                && (u.Status.Equals("Open") || u.Status.Equals("Lock"))
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
        public JsonResult DoDeleteUser(int id)
        {
            var result = db.Database.ExecuteSqlRaw(
                $"DeleteUser {id}");
            return Json(new { success = true });
        }

        public JsonResult DoDeleteUsers(List<int> ids)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                var result = db.Database.ExecuteSqlRaw(
                $"DeleteUser {ids[i]}");
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckLogin(string username, string password)
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

        public string Test()
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
            var usersTrash = db.Users.Where(
                u => u.Status.Equals("OnTrash")).OrderByDescending(u => u.Id).ToList();
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
                && u.Status.Equals("OnTrash")
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

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        // EXPORT TO EXCEL
        [HttpGet]
        public async Task<FileResult> ExportUserInExcel1(string type)
        {
            var user = new List<User>();
            var fileName = "";

            switch (type)
            {
                case "OpenLock":
                    user = await db.Users.Where(u => u.Status != "OnTrash").ToListAsync();
                    fileName = "users-active2.xlsx";
                    break;

                case "OnTrash":
                    user = await db.Users.Where(u => u.Status == "OnTrash").ToListAsync();
                    fileName = "users-trash.xlsx";
                    break;

                case "All":
                    user = await db.Users.ToListAsync();
                    fileName = "users-all.xlsx";
                    break;
            }

            return GenerateExcel(fileName, user);

        }

        private FileResult GenerateExcel(string fileName, IEnumerable<User> users)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Name"),
                new DataColumn("Age"),
                new DataColumn("Gender"),
                new DataColumn("Number"),
                new DataColumn("Username"),
                new DataColumn("Password"),
                new DataColumn("Priority"),
                new DataColumn("Status"),
                new DataColumn("Image"),
            });

            foreach (var user in users)
            {
                dataTable.Rows.Add(user.Id, user.Name, user.Age,
                    user.Gender, user.Number, user.Username,
                    user.Password, user.Priority, user.Status, user.Image);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable, fileName);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }

        // CONVERT IMAGE TO BASE64
        public static string ConvertToBase64(string imagePath)
        {
            try
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);
                string mimeType = GetMimeType(imagePath);
                return $"data:{mimeType};base64,{base64String}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image to base64: {ex.Message}");
                return null;
            }
        }

        private static string GetMimeType(string imagePath)
        {
            string extension = Path.GetExtension(imagePath).ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                default:
                    return "application/octet-stream";
            }
        }

        [HttpGet]
        public FileContentResult ExportCard1(string ids)
        {
            // Get img from project same name as image name in db
            HtmlToPdf oHtmlToPdf = new HtmlToPdf();

            oHtmlToPdf.Options.PdfPageSize = PdfPageSize.A4;
            oHtmlToPdf.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            oHtmlToPdf.Options.MarginLeft = 10;
            oHtmlToPdf.Options.MarginRight = 10;
            oHtmlToPdf.Options.MarginTop = 20;
            oHtmlToPdf.Options.MarginBottom = 20;

            List<User> users = new List<User>();
            PdfDocument opdfDocument = new PdfDocument();
            string[] lIds = ids.Split(",");

            for (int i = 0; i < lIds.Length; i++)
            {
                var id = Int32.Parse(lIds[i]);
                var user = db.Users.Find(id);
                string imgPath = "wwwroot\\uploads\\";
                string html = "";
                if (user != null)
                {
                    if (user.Image != null)
                    {
                        imgPath += user.Image;
                    }else
                    {
                        imgPath = "wwwroot\\img\\meobongbong.jpg";
                    }
                    
                    //Console.WriteLine(imgPath);
                    string img64base = ConvertToBase64(imgPath);
                    var idIn = RandomId(user.Id);
                    var name = user.Name.ToUpper();
                    var userRole = user.Priority.ToUpper();

                    html = "<body>\r\n    " +
                        "<style>\r\n        .body {\r\n            font-family: Helvetica !important;\r\n            font-size: 16px;\r\n            width: 100%;\r\n        }\r\n\r\n        .m-b-5 {\r\n            margin-bottom: 5px;\r\n        }\r\n\r\n        .container {\r\n            font-family: Helvetica !important;\r\n            font-size: 16px;\r\n            display: flex;\r\n            flex-direction: column;\r\n            gap: 100px;\r\n            justify-content: center;\r\n            width: 100%;\r\n            align-items: center;\r\n        }\r\n\r\n        .form {\r\n            width: 8.56cm;\r\n            height: 5.398cm;\r\n            background-color: #ffffff;\r\n            border: 1px solid #000000;\r\n            border-radius: 10px;\r\n            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);\r\n            padding: 20px;\r\n        }\r\n\r\n        .colored {\r\n            background: linear-gradient(135deg, #ffffff, #74b9ff);\r\n        }\r\n\r\n        .name-card {\r\n            text-align: center;\r\n            font-weight: bolder;\r\n            font-size: 2rem;\r\n            margin-bottom: 15px;\r\n        }\r\n\r\n        /* FRONT CARD */\r\n        .up-data {\r\n            display: flex;\r\n            flex-direction: row;\r\n        }\r\n\r\n        .text-bold {\r\n            font-weight: bolder;\r\n        }\r\n\r\n        .text-italic {\r\n            font-style: italic;\r\n        }\r\n\r\n        .information {\r\n            display: flex;\r\n            flex-direction: column;\r\n            gap: 5px;\r\n        }\r\n\r\n        .image {\r\n            width: 100px;\r\n            height: 100px;\r\n            background-color: aqua;\r\n            border-radius: 8px;\r\n            margin-right: 20px;\r\n        }\r\n\r\n            .image img {\r\n                width: 100px;\r\n                height: 100px;\r\n                object-fit: cover;\r\n                border-radius: 8px;\r\n            }\r\n\r\n        #name {\r\n            display: flex;\r\n            flex-direction: column;\r\n        }\r\n\r\n        .up-data {\r\n            margin-bottom: 10px;\r\n        }\r\n\r\n        .phone-number {\r\n            display: flex;\r\n            flex-direction: column;\r\n        }\r\n\r\n        .align-sc {\r\n            align-self: center;\r\n        }\r\n\r\n        /* BACK CARD */\r\n        .regulations ul {\r\n            text-decoration: none;\r\n            list-style: none;\r\n            padding: 0;\r\n        }\r\n\r\n        .text-jus {\r\n            text-align: justify;\r\n        }\r\n\r\n    </style>\r\n    <h1>USER: " +
                        user.Username +
                        "</h1>\r\n    <div class=\"container\">\r\n        <div id=\"front\" class=\"form colored\">\r\n            <div class=\"name-card\">ID CARD</div>\r\n            <div class=\"front-content\">\r\n                <div class=\"up-data\">\r\n                    <div class=\"image\">\r\n                        <img src=\"" + img64base + "\" />\r\n                    </div>\r\n                    <div class=\"information\">\r\n                        <div id=\"id-user m-b-5\">\r\n                            <span class=\"text-bold text-italic\">ID:</span>\r\n                            <span class=\"text-bold text-italic\">" +
                        idIn +
                        "</span>\r\n                        </div>\r\n                        <div id=\"name\">\r\n                            <span class=\"text-bold\">Name:</span>\r\n                            <span>" +
                        name +
                        "</span>\r\n                        </div>\r\n                        <div class=\"gender\">\r\n                            <span class=\"text-bold\">Gender:</span>\r\n                            <span>" +
                        user.Gender +
                        "</span>\r\n                        </div>\r\n                        <div class=\"role\">\r\n                            <span class=\"text-bold\">Role:</span>\r\n                            <span>" +
                        userRole +
                        "</span>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n                <div class=\"phone-number\">\r\n                    <span class=\"text-bold m-b-5\">Phone number:</span>\r\n                    <span class=\"align-sc\">" +
                        user.Number +
                        "</span>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <br />\r\n        <br />\r\n        <br />\r\n        <div id=\"back\" class=\"form colored\">\r\n            <div class=\"name-card\">REGULATIONS</div>\r\n            <div class=\"regulations\">\r\n                <ul>\r\n                    <li>\r\n                        <span>Lorem ipsum dolor sit amet, consectetur adipi scing elit.</span>\r\n                    </li>\r\n                    <br />\r\n                    <li>\r\n                        <span class=\"text-jus\">\r\n                            Lorem ipsum dolor sit amet, consectetur adipi elit. Maecenas\r\n                            sed scelerisque magna, acot porttitor sapien. Pellentesque\r\n                            rutrum, sapien vel aliquet vulputate, est est gravida purus,\r\n                            lobortis elementum diam diam non leo.\r\n                        </span>\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</body>";

                    PdfDocument pdfDocument = oHtmlToPdf.ConvertHtmlString(html);
                    opdfDocument.Append(pdfDocument);
                }
            }

            // Multiple Page
            byte[] pdf = opdfDocument.Save();
            opdfDocument.Close();

            return File(
                pdf,
                "application/pdf",
                "User.pdf");
        }

        public IActionResult ShowCardPdf()
        {
            return View("CardPdf");
        }

        private string RandomId(int id)
        {
            // Random id with 15 digit character
            string fullId = "";
            string strId = id.ToString();
            Random random = new Random();
            var length = 15 - strId.Length;
            for (int i = 0; i < length; i++)
            {
                fullId += random.Next(0, 9).ToString();
            }
            fullId += strId;
            return fullId;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
