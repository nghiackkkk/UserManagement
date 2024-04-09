using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Models.Viewsmodel;
using UserManagement.Services.Admin;

namespace UserManagement.Controllers
{
    public class TestController : Controller
    {
        UserManagement2Context db = new UserManagement2Context();

        //private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
        private readonly IAdminService _adminService;
        public TestController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("Test/CreateAddress")]
        public JsonResult CreateAddress([FromBody] Address address)
        {
            var isCreate = _adminService.CreateAddress(address);
            var addr = db.Addresses.FirstOrDefault(a => a.City == address.City);
            return Json(address.City);
        }


        [HttpPost("Test/UpdateUserDo")]
        public JsonResult UpdateUserDo(UserViewModel uvm)
        {
           
            return Json(uvm);
        }
    }
}
