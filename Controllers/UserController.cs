using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Models.Viewsmodel;
using UserManagement.Services.Admin;

namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        UserManagement2Context db = new UserManagement2Context();
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IAdminService _adminService;
        public UserController(ILogger<HomeController> logger,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,
            IAdminService adminService)
        {
            _logger = logger;
            _hostingEnvironment = environment;
            _adminService = adminService;
        }
        //SHOW VIEW
        public IActionResult Home()
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
                User user = _adminService.GetUserById(idUser);
                ViewBag.FullName = user.FullName;
                ViewBag.Cover = user.CoverImage;
                UserViewModel uvm = new UserViewModel
                {
                    Cover = user.CoverImage,
                    Username = user.Username,
                    Password = user.Password,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    EthnicGroup = user.EthnicGroup,
                    Religion = user.Regilion,
                    IdCard = user.IdCard,
                    CulturalStandard = user.CulturalStandard,
                };

                // Address
                Address pr = _adminService.GetAddressById((int)user.IdPernamentResidence);
                Address ra = _adminService.GetAddressById((int)user.IdRegularAddress);

                uvm.PermanentResidenceCity = pr.City;
                uvm.PermanentResidenceDistrict = pr.District;
                uvm.PermanentResidenceCommune = pr.Commune;
                uvm.PermanentResidenceAddress = pr.Address1;

                uvm.RegularAddressCity = ra.City;
                uvm.RegularAddressDistrict = ra.District;
                uvm.RegularAddressCommune = ra.Commune;
                uvm.RegularAddressAddress = ra.Address1;

                // Family
                List<Family> families = _adminService.GetFamilyByIdUser(idUser);
                foreach (Family family in families)
                {
                    SiblingViewModel sbp = new SiblingViewModel
                    {
                        Realtionship = family.Realtionship,
                        FullName = family.FullName,
                        YearOfBirth = family.YearOfBirth,
                        CurrentResident = family.CurrentResident,
                        CurrentOcupation = family.CurrentOcupation,
                        WorkingAgency = family.WorkingAgency
                    };
                    uvm.Siblings.Add(sbp);
                }
                // Study
                List<Studyprocess> spList = _adminService.GetStudyPByIdUser(idUser);
                foreach (Studyprocess sp in spList)
                {
                    StudyProcessViewModel spViewModel = new StudyProcessViewModel
                    {
                        StartTime = sp.StartTime,
                        EndTime = sp.EndTime,
                        SchoolUniversity = sp.SchoolUniversity,
                        ModeOfStudy = sp.ModeOfStudy
                    };
                    uvm.StudyProcesses.Add(spViewModel);
                }

                // Work
                List<Workingprocess> wpList = _adminService.GetWorkingPByIdUser(idUser);
                foreach (Workingprocess wp in wpList)
                {
                    WorkingProcessViewModel wpViewModel = new WorkingProcessViewModel
                    {
                        StartTime = wp.StartTime,
                        EndTime = wp.EndTime,
                        WorkingAgency = wp.WorkingAgency,
                        Position = wp.Position
                    };
                    uvm.WorkingProcesses.Add(wpViewModel);
                }

                return View("Index", uvm);
            }
            return RedirectToAction("Login", "Account");
        }

        //END SHOW VIEW
        public IActionResult PrintCard()
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            List<CardViewModel> lcvm = new List<CardViewModel>();
            DateTime currentDate = DateTime.Now;

            // Find by id 
            User user = _adminService.GetUserById(idUser);
            Address address = _adminService.GetAddressById((int)user.IdRegularAddress);
            var addr = address.Address1 + ", " + address.Commune + ", " + address.District + ", " + address.City;

            DateTime nextYear = currentDate.AddYears(1);
            string formattedDate = nextYear.ToString("dd/MM/yyyy");

            CardViewModel cvm = new CardViewModel
            {
                CoverImage = user.CoverImage,
                Fullname = user.FullName.ToUpper(),
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Address = addr,
                IdCard = user.IdCard,
                ExpireDate = formattedDate
            };
            lcvm.Add(cvm);


            string datenow = currentDate.ToString("MMMM d, yyyy"); // Formats the date as "April 5, 2024"
            ViewBag.DateNow = datenow;
            return View("~/Views/Admin/PrintCard.cshtml", lcvm);
        }

        public IActionResult PrintProfile()
        {
            List<UserViewModel> luvm = new List<UserViewModel>();
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            // Get user
            User user = _adminService.GetUserById(idUser);
            var addPR = "";
            if (user.IdPernamentResidence != null)
            {
                Address pr = _adminService.GetAddressById((int)user.IdPernamentResidence);
                addPR = pr.Address1 + ", " + pr.Commune + ", " + pr.District + ", " + pr.City;
            }
            var addRA = "";
            if (user.IdPernamentResidence != null)
            {
                Address pr = _adminService.GetAddressById((int)user.IdRegularAddress);
                addRA = pr.Address1 + ", " + pr.Commune + ", " + pr.District + ", " + pr.City;
            }

            //ViewBag.CoverImage = user.CoverImage;
            UserViewModel userVM = new UserViewModel
            {
                // Users
                Cover = user.CoverImage,
                FullName = user.FullName.ToUpper(),
                Gender = user.Gender,
                DateOfBirth = (System.DateOnly)user.DateOfBirth,
                IdCard = user.IdCard,
                PermanentResidenceAddress = addPR,
                RegularAddressAddress = addRA,
                PhoneNumber = user.PhoneNumber,
                EthnicGroup = user.EthnicGroup,
                Religion = user.Regilion,
                CulturalStandard = user.CulturalStandard
            };
            // Family
            List<Family> families = _adminService.GetFamilyByIdUser(user.Id);
            foreach (Family family in families)
            {
                SiblingViewModel sbp = new SiblingViewModel
                {
                    Realtionship = family.Realtionship,
                    FullName = family.FullName,
                    YearOfBirth = family.YearOfBirth,
                    CurrentResident = family.CurrentResident,
                    CurrentOcupation = family.CurrentOcupation,
                    WorkingAgency = family.WorkingAgency
                };
                userVM.Siblings.Add(sbp);
            }
            // Study
            List<Studyprocess> spList = _adminService.GetStudyPByIdUser(user.Id);
            foreach (Studyprocess sp in spList)
            {
                StudyProcessViewModel spViewModel = new StudyProcessViewModel
                {
                    StartTime = sp.StartTime,
                    EndTime = sp.EndTime,
                    SchoolUniversity = sp.SchoolUniversity,
                    ModeOfStudy = sp.ModeOfStudy
                };
                userVM.StudyProcesses.Add(spViewModel);
            }
            // Working
            List<Workingprocess> wpList = _adminService.GetWorkingPByIdUser(user.Id);
            foreach (Workingprocess wp in wpList)
            {
                WorkingProcessViewModel wpViewModel = new WorkingProcessViewModel
                {
                    StartTime = wp.StartTime,
                    EndTime = wp.EndTime,
                    WorkingAgency = wp.WorkingAgency,
                    Position = wp.Position
                };
                userVM.WorkingProcesses.Add(wpViewModel);
            }
            luvm.Add(userVM);

            return View("~/Views/Admin/PrintProfile.cshtml", luvm);
        }

        [HttpPost("User/UpdateUserDo")]
        public IActionResult UpdateUserDo(UserViewModel userViewsModel)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
            Console.WriteLine("-------------------------------------------------------------------");
            if (fullname != null && idUser != -1)
            {
                ViewBag.FullName = fullname;

                // Get User by id
                User userExist = _adminService.GetUserById(idUser);

                // Update address
                var addPR = db.Addresses
                    .Where(a => a.Id == userExist.IdPernamentResidence)
                    .FirstOrDefault();
                var addRA = db.Addresses
                   .Where(a => a.Id == userExist.IdRegularAddress)
                   .FirstOrDefault();

                addPR.City = userViewsModel.PermanentResidenceCity;
                addPR.District = userViewsModel.PermanentResidenceDistrict;
                addPR.Commune = userViewsModel.PermanentResidenceCommune;
                addPR.Address1 = userViewsModel.PermanentResidenceAddress;

                addRA.City = userViewsModel.RegularAddressCity;
                addRA.District = userViewsModel.RegularAddressDistrict;
                addRA.Commune = userViewsModel.RegularAddressCommune;
                addRA.Address1 = userViewsModel.RegularAddressAddress;

                _adminService.UpdateAddress(addPR);
                _adminService.UpdateAddress(addRA);

                // Update User
                User user = new User();
                var now = DateTime.Now;

                userExist.FullName = userViewsModel.FullName;
                userExist.Username = userViewsModel.Username;
                userExist.Password = userViewsModel.Password;
                userExist.Gender = userViewsModel.Gender;
                userExist.DateOfBirth = userViewsModel.DateOfBirth;
                userExist.PhoneNumber = userViewsModel.PhoneNumber;
                userExist.EthnicGroup = userViewsModel.EthnicGroup;
                userExist.Regilion = userViewsModel.Religion;
                userExist.IdCard = userViewsModel.IdCard;
                userExist.CulturalStandard = userViewsModel.CulturalStandard;
                userExist.DateModified = now;

                // Image
                if (userViewsModel.CoverImage != null)
                {
                    var uniqueFileName = GetUniqueFileName(userViewsModel.CoverImage.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        userViewsModel.CoverImage.CopyTo(fileStream);
                    }
                    userExist.CoverImage = uniqueFileName;
                    _adminService.UpdateUser(userExist);
                }
                else
                {
                    // Update user without cover image
                    _adminService.UpdateUserNoImage(userExist);
                }

                // Delete all and create new
                _adminService.DeleteStudyPByUserId(idUser);
                _adminService.DeleteWorkPByUserId(idUser);
                _adminService.DeleteSiblingsByUserId(idUser);

                // Get User ID
                var userId = idUser;

                // Create Family
                var countSib = userViewsModel.Siblings.Count();

                for (int i = 0; i < countSib; i++)
                {
                    Family sibling = ConstructFamily(userViewsModel.Siblings[i], userId);
                    _adminService.CreateFamily(sibling);
                }

                // Create Working
                var countW = userViewsModel.WorkingProcesses.Count();
                for (int i = 0; i < countW; i++)
                {
                    Workingprocess working = ConstructWorkingP(userViewsModel.WorkingProcesses[i], userId);
                    _adminService.CreateWorkingP(working);
                }

                // Create Studying
                var countS = userViewsModel.StudyProcesses.Count();
                for (int i = 0; i < countS; i++)
                {
                    Studyprocess stu = ConstructStudyingP(userViewsModel.StudyProcesses[i], userId);
                    _adminService.CreateStudyP(stu);
                }

                return Redirect("/User/Home");
            }
            return Redirect("/");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        private Studyprocess ConstructStudyingP(StudyProcessViewModel spvm, int userId)
        {
            Studyprocess study = new Studyprocess();
            study.StartTime = spvm.StartTime;
            study.EndTime = spvm.EndTime;
            study.SchoolUniversity = spvm.SchoolUniversity;
            study.ModeOfStudy = spvm.ModeOfStudy;
            study.IdUser = userId;

            return study;
        }

        private Workingprocess ConstructWorkingP(WorkingProcessViewModel workingProcessVM
            , int userId)
        {
            Workingprocess workingprocess = new Workingprocess();
            workingprocess.StartTime = workingProcessVM.StartTime;
            workingprocess.EndTime = workingProcessVM.EndTime;
            workingprocess.WorkingAgency = workingProcessVM.WorkingAgency;
            workingprocess.Position = workingProcessVM.Position;
            workingprocess.IdUser = userId;
            return workingprocess;
        }

        private Family ConstructFamily(SiblingViewModel siblingViewModel, int userId)
        {
            var family = new Family();
            family.Realtionship = siblingViewModel.Realtionship;
            family.FullName = siblingViewModel.FullName;
            family.YearOfBirth = siblingViewModel.YearOfBirth;
            family.CurrentResident = siblingViewModel.CurrentResident;
            family.CurrentOcupation = siblingViewModel.CurrentOcupation;
            family.WorkingAgency = siblingViewModel.WorkingAgency;
            family.IdUser = userId;
            return family;
        }
    }
}
