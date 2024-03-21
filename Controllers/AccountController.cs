using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        UserManagementContext db = new UserManagementContext();
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public AccountController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            hostingEnvironment = environment;
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


    }
}
