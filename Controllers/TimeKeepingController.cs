using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Office.Interop.Excel;
using UserManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UserManagement.Controllers
{
    [Route("Admin/[controller]")]
    public class TimeKeepingController : Controller
    {
        private readonly UserManagement2Context _db = new UserManagement2Context();
        public TimeKeepingController() { }
        private bool IsLogin()
        {
            var fullname = HttpContext.Session.GetString("FullName");
            if (fullname != null)
            {
                ViewBag.FullName = fullname;
                return true;
            }
            return false;
        }

        // SHOWVIEW
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Overview() { return View(); }

        [HttpGet("AssignExtraShift")]
        public IActionResult ShowAssignshift()
        {

            if (IsLogin())
            {

                return View();
            }
            return Redirect("/");
        }

        [HttpGet("AssignDepartment")]
        public IActionResult ShowAssignDepartment()
        {
            if (IsLogin())
            {

                return View();
            }
            return Redirect("/");
        }

        [HttpGet("ExtraShift")]
        public IActionResult ShowShift()
        {
            if (IsLogin())
            {

                return View();
            }
            return Redirect("/");
        }

        [HttpGet("TimeCheck")]
        public IActionResult ShowTimecheck()
        {
            if (IsLogin())
            {

                return View();
            }
            return Redirect("/");
        }

        [HttpGet("WorkingDay")]
        public IActionResult ShowWorkingday()
        {
            if (IsLogin())
            {
                return View();
            }
            return Redirect("/");
        }

        [HttpGet("RequestForLeave")]
        public IActionResult ShowRequestForLeave()
        {
            if (IsLogin())
            {
                return View();
            }
            return Redirect("/");
        }

        [HttpGet("PrintQRCheckIO")]
        public IActionResult PrintQRCheckIO()
        {
            return View();
        }
        // END SHOW VIEW

        // PROCCESS REQUESTS
        [HttpGet("API/GetDataAD")]
        public IActionResult GetDataAD()
        {
            // Get data for page assign department/position
            var result = from u in _db.Users
                         join s in _db.Staff on u.Id equals s.IdUser into staffGroup
                         from staff in staffGroup.DefaultIfEmpty()
                         select new
                         {
                             UserId = u.Id,
                             FullName = u.FullName,
                             DateOfBirth = u.DateOfBirth,
                             Department = staff != null ? staff.Department : null,
                             Position = staff != null ? staff.Position : null
                         };

            return Json(new { data = result });
        }

        [HttpGet("API/GetDataWD")]
        public IActionResult GetdataWD()
        {
            // Get data for page working day

            var result = from u in _db.Users
                         join s in _db.Staff on u.Id equals s.IdUser into staffGroup
                         from s in staffGroup.DefaultIfEmpty()
                         join w in _db.WorkingDays on s.Id equals w.IdStaff into workingDayGroup
                         from w in workingDayGroup.DefaultIfEmpty()
                         select new
                         {
                             StaffId = s != null ? s.Id : 0,
                             FullName = u.FullName,
                             Department = s != null ? s.Department : null,
                             Position = s != null ? s.Position : null,
                             Month = w != null ? w.Month : 0,
                             Year = w != null ? w.Year : 0,
                             NumberChecked = w != null ? w.NumberChecked : 0
                         };

            return Json(new { data = result });
        }

        [HttpGet("API/FilterWD")]
        public IActionResult FilterWD(string mon,
            string year, string dep, string pos)
        {
            var query = from u in _db.Users
                        join s in _db.Staff on u.Id equals s.IdUser into staffGroup
                        from staff in staffGroup.DefaultIfEmpty()
                        join w in _db.WorkingDays on staff.Id equals w.IdStaff into workingDayGroup
                        from workingDay in workingDayGroup.DefaultIfEmpty()
                        select new
                        {
                            StaffId = staff != null ? staff.Id : 0,
                            FullName = u.FullName,
                            Department = staff != null ? staff.Department : null,
                            Position = staff != null ? staff.Position : null,
                            Month = workingDay != null ? workingDay.Month : (int?)null,
                            Year = workingDay != null ? workingDay.Year : (int?)null,
                            NumberChecked = workingDay != null ? workingDay.NumberChecked : (int?)null
                        };
            if (mon != null)
            {
                query = query.Where(x => x.Month == Int32.Parse(mon));
            }

            if (year != null)
            {
                query = query.Where(x => x.Year == Int32.Parse(year));
            }

            if (dep != null)
            {
                query = query.Where(x => x.Department == dep);
            }

            if (pos != null)
            {
                query = query.Where(x => x.Position == pos);
            }

            var result = query.ToList();


            return Json(new { data = result });
        }

        [HttpGet("API/GetDataTC")]
        public IActionResult GetDataTC()
        {
            var query = from u in _db.Users
                        join s in _db.Staff on u.Id equals s.IdUser
                        join a in _db.AttendanceChecks on s.Id equals a.IdStaff
                        orderby a.Day descending
                        select new
                        {
                            id = s.Id,
                            id_staff = s.Id,
                            id_attendanceCheck = a.Id,
                            u.FullName,
                            s.Department,
                            s.Position,
                            a.Day,
                            time_in = a.TimeIn != null ? a.TimeIn.ToString() : "----/--/--",
                            time_out = a.TimeOut != null ? a.TimeOut.ToString() : "----/--/--",
                            a.Reason,
                            a.Accepted
                        };

            return Json(new { data = query });
        }

        [HttpGet("API/FilterTC")]
        public IActionResult filterTC(string day, string dep, string pos)
        {
            var query = from u in _db.Users
                        join s in _db.Staff on u.Id equals s.IdUser
                        join a in _db.AttendanceChecks on s.Id equals a.IdStaff
                        orderby a.Day descending
                        select new
                        {
                            s.Id,
                            u.FullName,
                            s.Department,
                            s.Position,
                            a.Day,
                            time_in = a.TimeIn != null ? a.TimeIn.ToString() : "----/--/--",
                            time_out = a.TimeOut != null ? a.TimeOut.ToString() : "----/--/--",
                            a.Reason,
                            a.Accepted
                        };

            if (day != null)
            {
                query = query.Where(x => x.Day.ToString() == day);
            }

            if (dep != null)
            {
                query = query.Where(x => x.Department == dep);
            }

            if (pos != null)
            {
                query = query.Where(x => x.Position == pos);
            }
            return Json(new { data = query });
        }

        [HttpGet("API/GetDataRFL")]
        public IActionResult GetDataRFL()
        {
            var result = from u in _db.Users
                         join s in _db.Staff on u.Id equals s.IdUser
                         join abs in _db.Absences on s.Id equals abs.IdStaff
                         orderby abs.Id descending
                         select new
                         {
                             id_user = u.Id,
                             id_staff = s.Id,
                             id_absence = abs.Id,
                             fullName = u.FullName,
                             department = s.Department,
                             position = s.Position,
                             absence_from = abs.DayFrom,
                             absence_to = abs.DayTo,
                             reason = abs.Reason,
                             accepted = abs.Accepted,
                         };
            return Json(new { data = result });
        }

        [HttpPost("API/UpdateStatusRFL")]
        public IActionResult UpdateStatusRFL([FromBody] UpdateStatusModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request body.");
            }

            if (string.IsNullOrEmpty(model.Status))
            {
                return BadRequest("Status or ID absence is null or empty.");
            }

            var absence = _db.Absences.Find(model.Id_absence);
            absence.Accepted = model.Status;
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet("API/UpdateStatusRFL1")]
        public IActionResult UpdateStatusRFL1(string id_absence)
        {
            var id_absence1 = Int32.Parse(id_absence);
            var absence = _db.Absences.Find(id_absence1);
            return Json(new { data = absence });
        }

        [HttpPost("API/RequestLate")]
        public IActionResult RequestLate([FromBody] UpdateStatusModel model)
        {
            var idAC = model.Id_absence;
            var attendanceCheck = _db.AttendanceChecks.Find(idAC);
            attendanceCheck.Accepted = model.Status;
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet("API/FilterRFL")]
        public IActionResult FilterRFL(string sta, string dep, string pos)
        {
            var result = from u in _db.Users
                         join s in _db.Staff on u.Id equals s.IdUser
                         join abs in _db.Absences on s.Id equals abs.IdStaff
                         orderby abs.DayFrom descending
                         select new
                         {
                             id_user = u.Id,
                             id_staff = s.Id,
                             id_absence = abs.Id,
                             fullName = u.FullName,
                             department = s.Department,
                             position = s.Position,
                             absence_from = abs.DayFrom,
                             absence_to = abs.DayTo,
                             reason = abs.Reason,
                             accepted = abs.Accepted,

                         };
            Console.WriteLine(sta);
            if (sta != null)
            {
                result = result.Where(a => a.accepted == sta);
            }

            if (dep != null)
            {
                result = result.Where(a => a.department == dep);
            }

            if (pos != null)
            {
                result = result.Where(a => a.position == pos);
            }

            return Json(new { data = result });
        }

        
        // END PROCCESS REQUESTS

        // POST ACTION
        [HttpPost("UpdateDepartment")]
        public IActionResult UpdateDepartment(string idUser, string department, string position)
        {
            var idUserI = Int32.Parse(idUser);
            var existingStaff = _db.Staff.FirstOrDefault(x => x.IdUser == idUserI);

            if (existingStaff == null)
            {
                Staff staff = new Staff
                {
                    IdUser = idUserI,
                    Department = department,
                    Position = position
                };

                _db.Staff.Add(staff);
            }
            else
            {
                existingStaff.Department = department;
                existingStaff.Position = position;
            }

            _db.SaveChanges();

            return RedirectToAction("ShowAssignDepartment");

        }
        // END POST ACTION
    }
    public class UpdateStatusModel
    {
        public string Status { get; set; }
        public int Id_absence { get; set; }
    }

}
