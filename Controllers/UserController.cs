using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing;
using System.Globalization;
using System.Net.NetworkInformation;
using UserManagement.Models;
using UserManagement.Models.Viewsmodel;
using UserManagement.Services.Admin;

namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        UserManagement2Context _db = new UserManagement2Context();
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
        private bool IsLogin()
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
                User user = _db.Users.FirstOrDefault(u => u.Id == idUser);
                ViewBag.FullName = fullname;
                ViewBag.Cover = user.CoverImage;
                return true;
            }
            return false;
        }
        private User InitialUserById()
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
            var user = _db.Users.FirstOrDefault(x => x.Id == idUser);
            ViewBag.IdUser = idUser;
            return user;
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

        [HttpGet("User/TimeKeeping")]
        public IActionResult ShowTimeKeeping()
        {
            if (IsLogin())
            {
                return View("~/Views/TimeKeeping/UserTimeKeeping.cshtml");
            }
            return Redirect("/");
        }

        [HttpGet("User/CheckIO")]
        public IActionResult CheckIO()
        {
            return View();
        }

        [HttpGet("User/Profile/Account")]
        public IActionResult ShowAccount()
        {
            if (IsLogin())
            {
                User user = InitialUserById();

                ViewBag.Username = user.Username;
                ViewBag.Password = user.Password;
                ViewBag.Img = user.CoverImage;

                return View("/Views/User/Profile/ShowAccount.cshtml");
            }
            return Redirect("/");
        }

        [HttpGet("User/Profile/Personal-Information")]
        public IActionResult ShowPersonalInfo()
        {
            if (IsLogin())
            {
                User user = InitialUserById();
                UserViewModel vm = new UserViewModel();

                // Address
                Address pr = _db.Addresses.FirstOrDefault(a => user.IdPernamentResidence == a.Id);
                Address ra = _db.Addresses.FirstOrDefault(a => a.Id == user.IdRegularAddress);

                vm.FullName = user.FullName;
                vm.Gender = user.Gender;
                vm.DateOfBirth = user.DateOfBirth;
                vm.PhoneNumber = user.PhoneNumber;
                vm.EthnicGroup = user.EthnicGroup;
                vm.Religion = user.Regilion;
                vm.IdCard = user.IdCard;
                vm.CulturalStandard = user.CulturalStandard;

                vm.PermanentResidenceCity = pr.City;
                vm.PermanentResidenceDistrict = pr.District;
                vm.PermanentResidenceCommune = pr.Commune;
                vm.PermanentResidenceAddress = pr.Address1;

                vm.RegularAddressCity = ra.City;
                vm.RegularAddressDistrict = ra.District;
                vm.RegularAddressCommune = ra.Commune;
                vm.RegularAddressAddress = ra.Address1;

                return View("/Views/User/Profile/ShowPersonalInfo.cshtml", vm);
            }
            return Redirect("/");
        }

        [HttpGet("User/Profile/Family-Members")]
        public IActionResult ShowFamilyMember()
        {
            if (IsLogin())
            {
                User user = InitialUserById();
                UserViewModel vm = new UserViewModel();

                List<Family> families = _db.Families.Where(x => x.IdUser == user.Id).ToList();
                foreach (Family family in families)
                {
                    SiblingViewModel sbp = new SiblingViewModel
                    {
                        Id = family.Id,
                        Realtionship = family.Realtionship,
                        FullName = family.FullName,
                        YearOfBirth = family.YearOfBirth,
                        CurrentResident = family.CurrentResident,
                        CurrentOcupation = family.CurrentOcupation,
                        WorkingAgency = family.WorkingAgency
                    };
                    vm.Siblings.Add(sbp);
                }

                return View("/Views/User/Profile/ShowFamilyMember.cshtml", vm);
            }
            return Redirect("/");
        }

        [HttpGet("User/Profile/Study-Process")]
        public IActionResult ShowStudyProcess()
        {
            if (IsLogin())
            {
                User user = InitialUserById();
                UserViewModel vm = new UserViewModel();

                List<Studyprocess> stp = _db.Studyprocesses
                    .Where(x => x.IdUser == user.Id).ToList();
                foreach (Studyprocess s in stp)
                {
                    StudyProcessViewModel sbp = new StudyProcessViewModel
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        SchoolUniversity = s.SchoolUniversity,
                        ModeOfStudy = s.ModeOfStudy
                    };
                    vm.StudyProcesses.Add(sbp);
                }

                return View("/Views/User/Profile/ShowStudyProcess.cshtml", vm);
            }
            return Redirect("/");
        }

        [HttpGet("User/Profile/Working-Process")]
        public IActionResult ShowWorkingProcess()
        {
            if (IsLogin())
            {
                User user = InitialUserById();
                UserViewModel vm = new UserViewModel();

                List<Workingprocess> stp = _db.Workingprocesses
                    .Where(x => x.IdUser == user.Id).ToList();
                foreach (Workingprocess s in stp)
                {
                    WorkingProcessViewModel sbp = new WorkingProcessViewModel
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        WorkingAgency = s.WorkingAgency,
                        Position = s.Position
                    };
                    vm.WorkingProcesses.Add(sbp);
                }

                return View("/Views/User/Profile/ShowWorkingProcess.cshtml", vm);
            }
            return Redirect("/");
        }

        [HttpGet("User/Profile/Preview")]
        public IActionResult ShowPreview()
        {
            if (IsLogin())
            {
                //ViewBag.WidthPreview = "style = \"width: 1000px !important\"";
                return View("/Views/User/Profile/ShowPreview.cshtml");
            }
            return Redirect("/");
        }
        //END SHOW VIEW



        // API
        [HttpGet("User/API/GetDataIO")]
        public IActionResult GetDataIO()
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
            var result = from u in _db.Users
                         join s in _db.Staff on u.Id equals s.IdUser
                         join a in _db.AttendanceChecks on s.Id equals a.IdStaff
                         where u.Id == idUser
                         orderby a.Day descending
                         select new
                         {
                             id_staff = s.Id,
                             id_user = u.Id,
                             fullName = u.FullName,
                             day = a.Day,
                             timeIn = a.TimeIn,
                             timeOut = a.TimeOut,
                             reason = a.Reason,
                             accepted = a.Accepted
                         };

            return Json(new { data = result });
        }

        [HttpGet("User/API/GetDataABS")]
        public IActionResult GetDataABS()
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
            var result = from u in _db.Users
                         join s in _db.Staff on u.Id equals s.IdUser
                         join a in _db.Absences on s.Id equals a.IdStaff
                         where u.Id == idUser
                         orderby a.Id descending
                         select new
                         {
                             id_staff = s.Id,
                             id_user = u.Id,
                             fullName = u.FullName,
                             day_from = a.DayFrom,
                             day_to = a.DayTo,
                             reason = a.Reason,
                             accepted = a.Accepted
                         };

            return Json(new { data = result });
        }

        [HttpPost("User/API/RemoveFam")]
        public IActionResult RemoveFam(int id)
        {
            Family fam = _db.Families.FirstOrDefault(f => f.Id == id);
            _db.Families.Remove(fam);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost("User/API/RemoveSW")]
        public IActionResult RemoveSW(int id, string table)
        {
            if (table == "study")
            {
                var stu = _db.Studyprocesses
                    .FirstOrDefault(s => s.Id == id);
                _db.Studyprocesses.Remove(stu);
            }
            else
            {
                var wor = _db.Workingprocesses
                    .FirstOrDefault(w => w.Id == id);
                _db.Workingprocesses.Remove(wor);
            }
            _db.SaveChanges();
            return Ok();
        }
        // END API



        // POST
        [HttpPost("User/DoCheckIO")]
        public IActionResult DoCheckIO(string username, string password)
        {
            List<User> users = _db.Users.ToList();

            User user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                if (user.Status != "Open")
                {
                    ViewBag.ErrorLogin = "User is locked";
                    return View("Login");
                }

                // check date time today if user have timein in table atendence_check
                var dateNow = DateTime.Now;
                var time = dateNow.ToString("hh:mm:ss");
                DateOnly dateOnly = new DateOnly(dateNow.Year, dateNow.Month, dateNow.Day);
                TimeOnly timeOnly = new TimeOnly(dateNow.Hour, dateNow.Minute, dateNow.Second);

                var result = from u in _db.Users
                             join s in _db.Staff on u.Id equals s.IdUser
                             join a in _db.AttendanceChecks on s.Id equals a.IdStaff
                             where a.Day == dateOnly && u.Id == user.Id
                             select new
                             {
                                 id_staff = s.Id,
                                 id_user = user.Id,
                                 full_name = user.FullName,
                                 day = a.Day,
                                 time_in = a.TimeIn,
                                 time_out = a.TimeOut
                             };
                var lResult = result.ToList();
                bool hasResult = result.Any();
                if (!hasResult || lResult.All(check => check.time_in != null && check.time_out != null))
                {
                    // if dont have result or (timein != null and timeput != null) to add new checkin
                    var idStaff = _db.Staff
                        .Where(s => s.IdUser == user.Id)
                        .Select(s => s.Id)
                        .FirstOrDefault();

                    if (idStaff != null)
                    {
                        AttendanceCheck ac = new AttendanceCheck
                        {
                            IdStaff = idStaff,
                            Day = dateOnly,
                            TimeIn = timeOnly
                        };
                        _db.AttendanceChecks.Add(ac);
                        _db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.ErrorLogin = "User is not a staff!";
                        return View("CheckIO");
                    }
                }
                else
                {
                    // if have time in then put to time out
                    foreach (var check in lResult)
                    {
                        if (check.time_in != null && check.time_out == null)
                        {
                            var attendanceCheck = _db.AttendanceChecks
                                .FirstOrDefault(ac => ac.IdStaff == check.id_staff && ac.Day == dateOnly && ac.TimeOut == null);

                            if (attendanceCheck != null)
                            {
                                attendanceCheck.TimeOut = timeOnly;
                                _db.SaveChanges();
                            }
                        }
                    }
                }

                return RedirectToAction("CheckIO", "User");
            }

            ViewBag.ErrorLogin = "Incorrect Username or Password!";
            return RedirectToAction("CheckIO", "User");

        }

        [HttpPost("User/CreateReason")]
        public IActionResult CreateReason(string reason, string day, string timein, string timeout)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
            var idStaff = _db.Staff.Where(s => s.IdUser == idUser).Select(a => a.Id).FirstOrDefault();
            var dayOnly = DateOnly.Parse(day);
            var timeinOnly = TimeOnly.Parse(timein);
            var timeoutOnly = TimeOnly.Parse(timeout);
            var attendanceCheck = _db.AttendanceChecks
                .FirstOrDefault(a =>
                    a.IdStaff == idStaff &&
                    a.Day == dayOnly &&
                    a.TimeIn == timeinOnly &&
                    a.TimeOut == timeoutOnly);

            attendanceCheck.Reason = reason;
            attendanceCheck.Accepted = "False";
            _db.SaveChanges();

            return RedirectToAction("ShowTimeKeeping", "User");
        }

        [HttpPost("User/CreateAbsence")]
        public IActionResult CreateAbsence(string dayfrom, string dayto, string reasonabs)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));
            var idStaff = _db.Staff.Where(s => s.IdUser == idUser).Select(a => a.Id).FirstOrDefault();

            var dayfromO = DateOnly.Parse(dayfrom);
            var daytoO = DateOnly.Parse(dayto);

            var absence = new Absence
            {
                IdStaff = idStaff,
                DayFrom = dayfromO,
                DayTo = daytoO,
                Reason = reasonabs,
                Accepted = "False"
            };

            _db.Absences.Add(absence);
            _db.SaveChanges();

            return RedirectToAction("ShowTimeKeeping", "User");
        }

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
                var addPR = _db.Addresses
                    .Where(a => a.Id == userExist.IdPernamentResidence)
                    .FirstOrDefault();
                var addRA = _db.Addresses
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

        [HttpPost("User/DoSaveAccount")]
        public IActionResult DoSaveAccount(UserViewModel userViewsModel)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            User userExist = _db.Users.FirstOrDefault(u => u.Id == idUser);
            if (userExist == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                userExist.Username = userViewsModel.Username;
                userExist.Password = userViewsModel.Password;
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
                }
            }
            _db.SaveChanges();
            TempData["SuccessMessage"] = "Account saved successfully.";
            return RedirectToAction("ShowAccount");
        }

        [HttpPost("User/DoSavePersonalInfo")]
        public IActionResult DoSavePersonalInfo(UserViewModel userViewsModel)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            User userExist = _db.Users.FirstOrDefault(u => u.Id == idUser);
            if (userExist == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                Address pr = _db.Addresses.FirstOrDefault(a => userExist.IdPernamentResidence == a.Id);
                Address ra = _db.Addresses.FirstOrDefault(a => a.Id == userExist.IdRegularAddress);

                userExist.FullName = userViewsModel.FullName;
                userExist.Gender = userViewsModel.Gender;
                userExist.DateOfBirth = userViewsModel.DateOfBirth;
                userExist.PhoneNumber = userViewsModel.PhoneNumber;
                userExist.EthnicGroup = userViewsModel.EthnicGroup;
                userExist.Regilion = userViewsModel.Religion;
                userExist.IdCard = userViewsModel.IdCard;
                userExist.CulturalStandard = userExist.CulturalStandard;

                pr.City = userViewsModel.PermanentResidenceCity;
                pr.District = userViewsModel.PermanentResidenceDistrict;
                pr.Commune = userViewsModel.PermanentResidenceCommune;
                pr.Address1 = userViewsModel.PermanentResidenceAddress;

                ra.City = userViewsModel.RegularAddressCity;
                ra.District = userViewsModel.RegularAddressDistrict;
                ra.Commune = userViewsModel.RegularAddressCommune;
                ra.Address1 = userViewsModel.RegularAddressAddress;
            }
            _db.SaveChanges();
            TempData["SuccessMessage"] = "Account saved successfully.";
            return RedirectToAction("ShowPersonalInfo");
        }

        [HttpPost("User/DoSaveFamilyMem")]
        public IActionResult DoSaveFamilyMem(UserViewModel userViewsModel)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            User userExist = _db.Users.FirstOrDefault(u => u.Id == idUser);
            if (userExist == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                foreach (var family in userViewsModel.Siblings)
                {
                    var famExist = _db.Families.FirstOrDefault(u => u.Id == family.Id);
                    if (famExist != null)
                    {
                        famExist.Realtionship = family.Realtionship;
                        famExist.FullName = family.FullName;
                        famExist.YearOfBirth = family.YearOfBirth;
                        famExist.CurrentResident = family.CurrentResident;
                        famExist.CurrentOcupation = family.CurrentOcupation;
                        famExist.WorkingAgency = family.WorkingAgency;
                    }
                    else
                    {
                        Family fam = new Family
                        {
                            Realtionship = family.Realtionship,
                            FullName = family.FullName,
                            YearOfBirth = family.YearOfBirth,
                            CurrentResident = family.CurrentResident,
                            CurrentOcupation = family.CurrentOcupation,
                            WorkingAgency = family.WorkingAgency
                        };
                    }
                    _db.SaveChanges();
                }
            }

            TempData["SuccessMessage"] = "Family members saved successfully.";
            return RedirectToAction("ShowFamilyMember");
        }

        [HttpPost("User/Profile/DoAddFamilyMem")]
        public IActionResult DoAddFamilyMem(string relationship, string fullname, int yob,
            string cr, string co, string wa)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            User userExist = _db.Users.FirstOrDefault(u => u.Id == idUser);
            if (userExist == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                Family family = new Family
                {
                    Realtionship = relationship,
                    FullName = fullname,
                    YearOfBirth = yob,
                    CurrentOcupation = cr,
                    CurrentResident = cr,
                    WorkingAgency = wa,
                    IdUser = idUser
                };
                _db.Families.Add(family);
                _db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Family members added successfully!";
            return RedirectToAction("ShowFamilyMember");
        }

        [HttpPost("User/Profile/DoAddStudyProcess")]
        public IActionResult DoAddStudyProcess(string startTime, string endTime,
            string su, string mot)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            User userExist = _db.Users.FirstOrDefault(u => u.Id == idUser);
            if (userExist == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                DateOnly startDateTime;
                DateOnly endDateTime;

                // Parse start time
                if (!DateOnly.TryParseExact(startTime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startDateTime))
                {
                    // Handle parsing error
                    return BadRequest("Invalid start time format");
                }

                // Parse end time
                if (!DateOnly.TryParseExact(endTime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endDateTime))
                {
                    // Handle parsing error
                    return BadRequest("Invalid end time format");
                }
                Studyprocess sp = new Studyprocess
                {
                    StartTime = startDateTime,
                    EndTime = endDateTime,
                    SchoolUniversity = su,
                    ModeOfStudy = mot,
                    IdUser = idUser
                };
                _db.Studyprocesses.Add(sp);
                _db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Studyprocess added successfully!";
            return RedirectToAction("ShowStudyProcess");
        }

        [HttpPost("User/Profile/DoAddWorkingProcess")]
        public IActionResult DoAddWorkingProcess(string startTime, string endTime,
            string wag, string pos)
        {
            var idUser = Int32.Parse(HttpContext.Session.GetString("IDUserLogin"));

            User userExist = _db.Users.FirstOrDefault(u => u.Id == idUser);
            if (userExist == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                DateOnly startDateTime;
                DateOnly endDateTime;

                // Parse start time
                if (!DateOnly.TryParseExact(startTime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startDateTime))
                {
                    // Handle parsing error
                    return BadRequest("Invalid start time format");
                }

                // Parse end time
                if (!DateOnly.TryParseExact(endTime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endDateTime))
                {
                    // Handle parsing error
                    return BadRequest("Invalid end time format");
                }
                Workingprocess sp = new Workingprocess
                {
                    StartTime = startDateTime,
                    EndTime = endDateTime,
                    WorkingAgency = wag,
                    Position = pos,
                    IdUser = idUser
                };
                _db.Workingprocesses.Add(sp);
                _db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Workingprocess added successfully!";
            return RedirectToAction("ShowWorkingProcess");
        }
        // END POST
    }

    public class AccountViewModel()
    {
        public string? usn { get; set; }
        public string? pas { get; set; }
        public IFormFile? img { get; set; }
    }
}
