using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Geom;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using UserManagement.Models;
using UserManagement.Services.Admin;
using System.IO;
using Path = System.IO.Path;
using UserManagement.Models.Viewsmodel;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Crypto.Prng;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Web;
using OfficeOpenXml.Style;


namespace UserManagement.Controllers
{
    public class AdminController : Controller
    {
        UserManagement2Context db = new UserManagement2Context();

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _adminService = adminService;
            _hostingEnvironment = hostingEnvironment;
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
                    if (user.CoverImage != null && user.CoverImage == name)
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    string imagePath = Path.Combine(directPath, name);
                    DeleteFile(imagePath);
                }
            }

            return true;
        }

        private void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                }
                System.IO.File.Delete(path);
            }
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
        public IActionResult ShowUser(string search = "", string type = "active",
            string sort = "latest", int pageNumber = 1, int pageSize = 10)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                //RemoveUnusedImage();
                ViewBag.FullName = fullname;
                var users = _adminService.GetUserByType(search, type, sort, pageNumber, pageSize);
                var allActiveUser = db.Users.Where(u => u.InTrash == "False").Count();
                ViewBag.SortType = sort;

                int totalUser = allActiveUser;
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
                var allTrashUser = db.Users.Where(u => u.InTrash == "True").Count();
                int totalUser = allTrashUser;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPage = Math.Ceiling((double)totalUser / pageSize);
                ViewBag.TotalUsers = totalUser;

                return View(users);
            }
            return Redirect("/");

        }
        
        public IActionResult AddUser()
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;


                return View();
            }
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult UpdateUser(int id)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                HttpContext.Session.SetString("EditId", id.ToString());
                ViewBag.FullName = fullname;
                ViewBag.Id = id;
                UserViewModel model = new UserViewModel();
                // Find User by id
                User user = _adminService.GetUserById(id);
                //Find address by user id
                Address pr = _adminService.GetAddressById((int)user.IdPernamentResidence);
                Address ra = _adminService.GetAddressById((int)user.IdRegularAddress);
                // User
                ViewBag.CoverImage = user.CoverImage;
                model.Username = user.Username;
                model.Password = user.Password;
                model.FullName = user.FullName;
                model.Gender = user.Gender;
                model.DateOfBirth = (System.DateOnly)user.DateOfBirth;
                model.PhoneNumber = user.PhoneNumber;
                model.EthnicGroup = user.EthnicGroup;
                model.Religion = user.Regilion;
                model.IdCard = user.IdCard;
                model.CulturalStandard = user.CulturalStandard;

                model.PermanentResidenceCity = pr.City;
                model.PermanentResidenceDistrict = pr.District;
                model.PermanentResidenceCommune = pr.Commune;
                model.PermanentResidenceAddress = pr.Address1;

                model.RegularAddressCity = ra.City;
                model.RegularAddressDistrict = ra.District;
                model.RegularAddressCommune = ra.Commune;
                model.RegularAddressAddress = ra.Address1;
                // Family
                List<Family> families = _adminService.GetFamilyByIdUser(id);
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
                    model.Siblings.Add(sbp);
                }
                // Study
                List<Studyprocess> spList = _adminService.GetStudyPByIdUser(id);
                foreach (Studyprocess sp in spList)
                {
                    StudyProcessViewModel spViewModel = new StudyProcessViewModel
                    {
                        StartTime = sp.StartTime,
                        EndTime = sp.EndTime,
                        SchoolUniversity = sp.SchoolUniversity,
                        ModeOfStudy = sp.ModeOfStudy
                    };
                    model.StudyProcesses.Add(spViewModel);
                }
                // Working
                List<Workingprocess> wpList = _adminService.GetWorkingPByIdUser(id);
                foreach (Workingprocess wp in wpList)
                {
                    WorkingProcessViewModel wpViewModel = new WorkingProcessViewModel
                    {
                        StartTime = wp.StartTime,
                        EndTime = wp.EndTime,
                        WorkingAgency = wp.WorkingAgency,
                        Position = wp.Position
                    };
                    model.WorkingProcesses.Add(wpViewModel);
                }

                return View(model);
            }
            return Redirect("/");
        }

        public IActionResult PrintDasboard()
        {
            // Overview data
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            var total = db.Users.Count();
            var totalMale = db.Users.Where(u => u.Gender == "Male").Count();
            var totalFemale = db.Users.Where(u => u.Gender == "Female").Count();
            var newToday = db.Users.Where(u => u.DateCreated >= today && u.DateCreated < tomorrow).Count();

            // New user past 7 day
            Dictionary<string, int> data = new Dictionary<string, int>();
            for (int i = 0; i < 7; i++)
            {
                DateTime pastDay = today.AddDays(-i - 1);
                string dayMonth = pastDay.ToString("dd/MM");

                int newUserCount = GetNewUserCountForDay(pastDay);
                data.Add(dayMonth, newUserCount);
            }

            Dictionary<string, int> data0 = new Dictionary<string, int>();
            for (int i = data.Count() - 1; i >= 0; i--)
            {
                var kvp = data.ElementAt(i);
                data0.Add(kvp.Key, kvp.Value);
            }

            // User by province

            //
            ReportViewModel model = new ReportViewModel
            {
                Total = total,
                TotalMale = totalMale,
                TotalFemale = totalFemale,
                NewToday = newToday,

                Data7 = data
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult PrintCard(List<int> listId)
        {
            var listIdLe = listId.Count();
            List<CardViewModel> lcvm = new List<CardViewModel>();
            DateTime currentDate = DateTime.Now;
            for (var i = 0; i < listIdLe; i++)
            {
                // Find by id 
                User user = _adminService.GetUserById(listId[i]);
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
            }

            string datenow = currentDate.ToString("MMMM d, yyyy"); // Formats the date as "April 5, 2024"
            ViewBag.DateNow = datenow;
            return View(lcvm);
        }

        public IActionResult PrintAllCard()
        {
            List<User> users = db.Users.ToList();
            var listIdLe = users.Count();
            List<CardViewModel> lcvm = new List<CardViewModel>();
            DateTime currentDate = DateTime.Now;
            for (var i = 0; i < listIdLe; i++)
            {
                // Find by id 
                User user = users[i];
                var addr = "";
                if (user.IdRegularAddress != null)
                {
                    Address address = _adminService.GetAddressById((int)user.IdRegularAddress);
                    addr = address.Address1 + ", " + address.Commune + ", " + address.District + ", " + address.City;
                }

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
            }

            string datenow = currentDate.ToString("MMMM d, yyyy"); // Formats the date as "April 5, 2024"
            ViewBag.DateNow = datenow;
            return View("PrintCard", lcvm);
        }

        [HttpGet]
        public IActionResult PrintProfile(List<int> ids)
        {
            List<UserViewModel> luvm = new List<UserViewModel>();
            for (var i = 0; i < ids.Count(); i++)
            {
                // Get user
                User user = _adminService.GetUserById(ids[i]);
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
            }
            return View(luvm);
        }
        
        [HttpGet]
        public IActionResult PrintAllProfile()
        {
            List<UserViewModel> luvm = new List<UserViewModel>();
            List<int> ids = db.Users.Select(x => x.Id).ToList();
            for (var i = 0; i < ids.Count(); i++)
            {
                // Get user
                User user = _adminService.GetUserById(ids[i]);
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
                    Id = ids[i],
                    Cover = user.CoverImage,
                    FullName = user.FullName.ToUpper(),
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth.HasValue ? (System.DateOnly)user.DateOfBirth : default(System.DateOnly),
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
            }
            return View("PrintProfile", luvm);
        }
        
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

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        [HttpPost("Admin/CreateUser")]
        public IActionResult CreateUser(Models.Viewsmodel.UserViewModel userViewsModel)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;

                // Create Address
                Address addressPR = new Address();
                addressPR.City = userViewsModel.PermanentResidenceCity;
                addressPR.District = userViewsModel.PermanentResidenceDistrict;
                addressPR.Commune = userViewsModel.PermanentResidenceCommune;
                addressPR.Address1 = userViewsModel.PermanentResidenceAddress;
                Address addressRA = new Address();
                addressRA.City = userViewsModel.RegularAddressCity;
                addressRA.District = userViewsModel.RegularAddressDistrict;
                addressRA.Commune = userViewsModel.RegularAddressCommune;
                addressRA.Address1 = userViewsModel.RegularAddressAddress;
                _adminService.CreateAddress(addressPR);
                _adminService.CreateAddress(addressRA);

                // Get Address ID
                var idPR = db.Addresses
                    .Where(a => a.City == addressPR.City
                        && a.District == addressPR.District
                        && a.Commune == addressPR.Commune
                        && a.Address1.Equals(addressPR.Address1))
                    .Select(a => a.Id)
                    .FirstOrDefault();
                var idRA = db.Addresses
                    .Where(a => a.City == addressRA.City
                        && a.District == addressRA.District
                        && a.Commune == addressRA.Commune
                        && a.Address1.Equals(addressRA.Address1))
                    .Select(a => a.Id)
                    .FirstOrDefault();

                // Create User
                User user = new User();
                var now = DateTime.Now;
                user.FullName = userViewsModel.FullName;
                user.Username = userViewsModel.Username;
                user.Password = userViewsModel.Password;
                user.Status = "Open";
                user.Priority = "User";
                user.Gender = userViewsModel.Gender;
                user.DateOfBirth = userViewsModel.DateOfBirth;
                user.PhoneNumber = userViewsModel.PhoneNumber;
                user.EthnicGroup = userViewsModel.EthnicGroup;
                user.Regilion = userViewsModel.Religion;
                user.IdCard = userViewsModel.IdCard;
                user.CulturalStandard = userViewsModel.CulturalStandard;
                user.InTrash = "False";
                user.IdPernamentResidence = idPR;
                user.IdRegularAddress = idRA;
                user.DateCreated = now;
                user.DateModified = now;
                // Image
                var uniqueFileName = GetUniqueFileName(userViewsModel.CoverImage.FileName);
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                userViewsModel.CoverImage.CopyTo(new FileStream(filePath, FileMode.Create));
                user.CoverImage = uniqueFileName;

                _adminService.CreateUser(user);

                // Get User ID
                var userId = db.Users
                    .Where(u => u.Username.Equals(user.Username))
                    .Select(u => u.Id)
                    .FirstOrDefault();

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

                return Redirect("/Admin/ShowUser");
            }
            return Redirect("/");
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

        [HttpPost("Admin/UpdateUserDo")]
        public IActionResult UpdateUserDo(UserViewModel userViewsModel)
        {
            var fullname = HttpContext.Session.GetString("FullName");
            var idUser = Int32.Parse(HttpContext.Session.GetString("EditId"));
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

                return Redirect("/Admin/ShowUser");
            }
            return Redirect("/");
        }

        [HttpGet]
        public JsonResult CheckUsernameU(string username, int id)
        {
            var idUser = id;
            User userOld = _adminService.GetUserById(idUser);
            List<User> lUser = db.Users
                .Where(u => u.Username == username && u.Username != userOld.Username)
                .ToList();
            if (username == userOld.Username || lUser.Count() == 0)
            {
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        [HttpGet]
        public JsonResult CheckUsername(string username)
        {
            if (_adminService.CheckUsername(username) == true)
            {
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        // DATA FOR DASBOARD
        [HttpGet("Admin/Chart/NewUserPast7")]
        public JsonResult NewUserPast7()
        {
            // Get number of users for each past 7 days
            Dictionary<string, int> data = new Dictionary<string, int>();
            DateTime today = DateTime.Today;

            // Loop through past 7 days
            for (int i = 0; i < 7; i++)
            {
                DateTime pastDay = today.AddDays(-i - 1);
                string dayMonth = pastDay.ToString("dd/MM");

                int newUserCount = GetNewUserCountForDay(pastDay);
                data.Add(dayMonth, newUserCount);
            }

            Dictionary<string, int> data0 = new Dictionary<string, int>();
            for (int i = data.Count() - 1; i >= 0; i--)
            {
                var kvp = data.ElementAt(i);
                data0.Add(kvp.Key, kvp.Value);
            }
            return Json(new { result = true, data = data0 });
        }

        private int GetNewUserCountForDay(DateTime date)
        {
            var count = db.Users
                .Where(u => EF.Functions.DateDiffDay(u.DateCreated, date.Date) == 0)
                .Count();

            if (count > 0)
            {
                return count;
            }
            Random random = new Random();
            return random.Next(1, 10);
        }
        // END DATA FOR DASBOARD

        public IActionResult ExportExcel()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "Files", "User_Data.xlsx");
            FileInfo existingFile = new FileInfo(filePath);

            ExcelPackage excel = new ExcelPackage(existingFile);
            // Sheet User
            var workSheet = excel.Workbook.Worksheets[0];
            workSheet.Cells[2, 1].Value = "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyy");
            var stt = 1;

            var lst = db.Users.ToList();
            var i = 6;

            foreach (var item in lst)
            {
                workSheet.Cells[i, 1].Value = stt;
                workSheet.Cells[i, 2].Value = item.Id;
                workSheet.Cells[i, 3].Value = item.FullName;
                workSheet.Cells[i, 4].Value = item.DateOfBirth;
                workSheet.Cells[i, 5].Value = item.Gender;
                workSheet.Cells[i, 6].Value = item.IdCard;
                workSheet.Cells[i, 7].Value = item.PhoneNumber;
                workSheet.Cells[i, 8].Value = item.IdPernamentResidence;
                workSheet.Cells[i, 9].Value = item.IdRegularAddress;
                workSheet.Cells[i, 10].Value = item.CulturalStandard;
                workSheet.Cells[i, 11].Value = item.Regilion;
                workSheet.Cells[i, 12].Value = item.EthnicGroup;
                i++;
                stt++;
            }
            var modelTable = workSheet.Cells[6, 1, 6 + lst.Count, 12];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            // Sheet Address
            var workSheet1 = excel.Workbook.Worksheets[1];
            workSheet1.Cells[2, 1].Value = "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyy");
            stt = 1;

            var lst1 = db.Addresses.ToList();
            i = 6;

            foreach (var item in lst1)
            {
                workSheet1.Cells[i, 1].Value = stt;
                workSheet1.Cells[i, 2].Value = item.Id;
                workSheet1.Cells[i, 3].Value = item.City;
                workSheet1.Cells[i, 4].Value = item.District;
                workSheet1.Cells[i, 5].Value = item.Commune;
                workSheet1.Cells[i, 6].Value = item.Address1;
                i++;
                stt++;
            }
            var modelTable1 = workSheet1.Cells[6, 1, 6 + lst1.Count, 6];
            modelTable1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            // Sheet Family
            var workSheet3 = excel.Workbook.Worksheets[2];
            workSheet3.Cells[2, 1].Value = "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyy");
            stt = 1;

            var lst3 = db.Families.ToList();
            i = 6;

            foreach (var item in lst3)
            {
                workSheet3.Cells[i, 1].Value = stt;
                workSheet3.Cells[i, 2].Value = item.Id;
                workSheet3.Cells[i, 3].Value = item.Realtionship;
                workSheet3.Cells[i, 4].Value = item.FullName;
                workSheet3.Cells[i, 5].Value = item.YearOfBirth;
                workSheet3.Cells[i, 6].Value = item.CurrentResident;
                workSheet3.Cells[i, 7].Value = item.CurrentOcupation;
                workSheet3.Cells[i, 8].Value = item.WorkingAgency;
                workSheet3.Cells[i, 9].Value = item.IdUser;
                i++;
                stt++;
            }
            var modelTable3 = workSheet3.Cells[6, 1, 6 + lst3.Count, 9];
            modelTable3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            // Sheet Study Process
            var workSheet4 = excel.Workbook.Worksheets[3];
            workSheet4.Cells[2, 1].Value = "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyy");
            stt = 1;

            var lst4 = db.Studyprocesses.ToList();
            i = 6;

            foreach (var item in lst4)
            {
                workSheet4.Cells[i, 1].Value = stt;
                workSheet4.Cells[i, 2].Value = item.Id;
                workSheet4.Cells[i, 3].Value = item.StartTime;
                workSheet4.Cells[i, 4].Value = item.EndTime;
                workSheet4.Cells[i, 5].Value = item.SchoolUniversity;
                workSheet4.Cells[i, 6].Value = item.ModeOfStudy;
                workSheet4.Cells[i, 7].Value = item.IdUser;
                i++;
                stt++;
            }
            var modelTable4 = workSheet4.Cells[6, 1, 6 + lst4.Count, 7];
            modelTable4.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable4.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable4.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable4.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            // Sheet Working Process
            var workSheet5 = excel.Workbook.Worksheets[4];
            workSheet5.Cells[2, 1].Value = "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyy");
            stt = 1;

            var lst5 = db.Workingprocesses.ToList();
            i = 6;

            foreach (var item in lst5)
            {
                workSheet5.Cells[i, 1].Value = stt;
                workSheet5.Cells[i, 2].Value = item.Id;
                workSheet5.Cells[i, 3].Value = item.StartTime;
                workSheet5.Cells[i, 4].Value = item.EndTime;
                workSheet5.Cells[i, 5].Value = item.WorkingAgency;
                workSheet5.Cells[i, 6].Value = item.Position;
                workSheet5.Cells[i, 7].Value = item.IdUser;
                i++;
                stt++;
            }
            var modelTable5 = workSheet5.Cells[6, 1, 6 + lst5.Count, 7];
            modelTable5.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable5.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable5.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable5.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            var memoryStream = new MemoryStream();
            excel.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "User_Data (2).xlsx");
        }
    }

}
