using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Geom;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using UserManagement.Models;
using UserManagement.Services.Admin;

namespace UserManagement.Controllers
{
    public class AdminController : Controller
    {
        UserManagement2Context db = new UserManagement2Context();

        //private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        //SHOW VIEW
        public IActionResult Home()
        {
            if (HttpContext.Session.GetString("FullName") != null)
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);

                ViewBag.FullName = HttpContext.Session.GetString("FullName").ToString();
                TempData["Total"] = db.Users.Count();
                TempData["NewUser"] = db.Users.Where(u => u.DateCreated >= today && u.DateCreated < tomorrow).Count();
                TempData["MaleCount"] = db.Users.Where(u => u.Gender == "Male").Count();
                TempData["FemaleCount"] = db.Users.Where(u => u.Gender == "Female").Count();

                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ShowUser(string search = "", string type = "active", string sort = "latest", int pageNumber = 1, int pageSize = 10)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;
                var users = _adminService.GetUserByType(search, type, sort, pageNumber, pageSize);
                ViewBag.SortType = sort;
                int totalUser = users.Count();
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPage = Math.Ceiling((double)totalUser / pageSize);
                ViewBag.TotalUsers = totalUser;

                return View(users);
            }
            return Redirect("/");
        }
        [HttpGet]
        public JsonResult ShowUserJson(string search = "", string type = "active", string sort = "latest", int pageNumber = 1, int pageSize = 10)
        {
            var users = _adminService.GetUserByType(search, type, sort, pageNumber, pageSize);

            int totalUser = users.Count();
            var paginate = Json(new
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                totalPage = Math.Ceiling((double)totalUser / pageSize),
                totalUsers = totalUser
            });

            return Json(new
            {
                result = true,
                users = users,
                paginate = paginate
            });
        }

        public IActionResult ShowTrash(string search = "", string type = "trash", string sort = "latest", int pageNumber = 1, int pageSize = 10)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;
                var users = _adminService.GetUserByType(search, type, sort, pageNumber, pageSize);
                ViewBag.SortType = sort;
                int totalUser = users.Count();
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPage = Math.Ceiling((double)totalUser / pageSize);
                ViewBag.TotalUsers = totalUser;

                return View(users);
            }
            return Redirect("/");

        }
        public IActionResult AddUser() {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;


                return View();
            }
            return Redirect("/");
        }
        public IActionResult UpdateUser() { return View(); }
        //END SHOW VIEW
        public void SetPaginate(int pageNumber, int pageSize, int totalUser)
        {

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPage = Math.Ceiling((double)totalUser / pageSize);
            ViewBag.TotalUsers = totalUser;
        }

        [HttpGet]
        public JsonResult ToggleStatus(int id)
        {
            _adminService.ToggleStatus(id);
            return Json(new { result = true });
        }

        [HttpGet]
        public IActionResult SearchUser(string search, string type,
            string sort, int pageNumber = 1, int pageSize = 10)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;
                var users = _adminService.SearchUser(search, sort, type,
                    pageNumber, pageSize);

                SetPaginate(pageNumber, pageSize, users.Count());
                var view = (type == "trash") ? "ShowTrash" : "ShowUser";
                return View(view, users);
            }
            return Redirect("/");
        }

        [HttpPost("Admin/ToggleTrash")]
        public JsonResult ToggleTrash(int id)
        {
            _adminService.MoveToTrash(id);
            return Json(new { result = true });
        }

        [HttpPost("Admin/ResetPassword")]
        public JsonResult ResetPassword(List<int> ids)
        {
            var length = ids.Count();
            for (var i = 0; i < length; i++)
            {
                _adminService.ResetPassword(ids[i]);
            }
            return Json(new { result = true });
        }

        [HttpPost("Admin/DeletePernament")]
        public JsonResult DeletePernament(List<int> ids)
        {
            var length = ids.Count();
            for (var i = 0; i < length; i++)
            {
                _adminService.DeletePernament(ids[i]);
            }
            return Json(new { result = true });
        }

        [HttpPost("Admin/CreateUser")]
        public IActionResult CreateUser()
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;


                return View();
            }
            return Redirect("/");
        }
    }
}
