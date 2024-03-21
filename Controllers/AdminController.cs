using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class AdminController : Controller
    {
        UserManagementContext db = new UserManagementContext();
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public AdminController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            hostingEnvironment = environment;
        }
        //SHOW VIEW
        public IActionResult Home() { return View(); }
        public IActionResult ShowUser() { return View(); }
        public IActionResult ShowTrash() { return View(); }
        public IActionResult AddUser() { return View(); }
        public IActionResult UpdateUser() { return View(); }
        //END SHOW VIEW
    }
}
