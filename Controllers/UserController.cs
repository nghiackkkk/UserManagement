using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        UserManagement2Context db = new UserManagement2Context();
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public UserController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            hostingEnvironment = environment;
        }
        //SHOW VIEW
        public IActionResult Index() { return View(); }

        //END SHOW VIEW
    }
}
