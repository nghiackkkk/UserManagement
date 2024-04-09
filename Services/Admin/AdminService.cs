using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManagement2Context _context;

        public AdminService(UserManagement2Context context)
        {
            _context = context;
        }

        public bool CheckUsername(string username)
        {
            var result = _context.Users.FromSqlInterpolated(
                $"EXEC CheckUsernameExists @Username={username}")
                .AsEnumerable()
                .ToList();
            foreach (var entity in result)
            {
                if (entity.Id != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CreateAddress(Address address)
        {
            if (address != null)
            {
                _context.Database
                    .ExecuteSqlInterpolated(
                    $@"EXEC CreateAddress @city={address.City}, @district={address.District}, @commune={address.Commune}, @address={address.Address1}");
                return true;
            }
            return false;
        }

        public bool CreateFamily(Family family)
        {
            if (family != null)
            {
                try
                {
                    _context.Database.ExecuteSqlInterpolated($@"
                        EXEC [dbo].[CreateFamily]
                            @relationS = {family.Realtionship},
                            @fullname = {family.FullName},
                            @yearOfBirth = {family.YearOfBirth},
                            @currentRedident = {family.CurrentResident},
                            @currentOcupation = {family.CurrentOcupation},
                            @workingAgency = {family.WorkingAgency},
                            @idUser = {family.IdUser}
                        ");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating family: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public bool CreateStudyP(Studyprocess sp)
        {
            if (sp != null)
            {
                try
                {
                    _context.Database.ExecuteSqlInterpolated($@"
                        EXEC [dbo].[CreateStudyP]
                            @startT = {sp.StartTime},
                            @endT = {sp.EndTime},
                            @school = {sp.SchoolUniversity},
                            @mode = {sp.ModeOfStudy},
                            @idUser = {sp.IdUser}
                    ");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating studying process: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public bool CreateUser(User user)
        {
            if (user != null)
            {
                try
                {
                    _context.Database.ExecuteSqlInterpolated($@"
                        EXEC [dbo].[CreateUser]
                            @fullname = {user.FullName},
                            @image = {user.CoverImage},
                            @username = {user.Username},
                            @password = {user.Password},
                            @gender = {user.Gender},
                            @dateObBirth = {user.DateOfBirth},
                            @phoneNumber = {user.PhoneNumber},
                            @ethnicGroup = {user.EthnicGroup},
                            @regilion = {user.Regilion},
                            @idCard = {user.IdCard},
                            @cultralStandard = {user.CulturalStandard},
                            @idPR = {user.IdPernamentResidence},
                            @idRA = {user.IdRegularAddress},
                            @dateCreated = {user.DateCreated},
                            @dateModified = {user.DateModified},
                            @status = {user.Status},
                            @priority = {user.Priority},
                            @inTrash = {user.InTrash}
                    ");
                    return true;
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error creating user: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public bool CreateWorkingP(Workingprocess workingprocess)
        {
            if (workingprocess != null)
            {
                try
                {
                    _context.Database.ExecuteSqlInterpolated($@"
                        EXEC [dbo].[CreateWorkingP]
                            @startT = {workingprocess.StartTime},
                            @endT = {workingprocess.EndTime},
                            @workingAgency = {workingprocess.WorkingAgency},
                            @position = {workingprocess.Position},
                            @idUser = {workingprocess.IdUser}
                    ");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating working process: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public void DeletePernament(int id)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC DeletePernament @id = {id}");
        }

        public void DeleteSiblingsByUserId(int idUser)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC DeleteSibling @idUser = {idUser}");
        }

        public void DeleteStudyPByUserId(int idUser)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC DeleteStudy @idUser = {idUser}");
        }

        public void DeleteWorkPByUserId(int idUser)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC DeleteWork @idUser = {idUser}");
        }

        public Address GetAddressById(int id)
        {
            return _context.Addresses
                 .FromSqlInterpolated($"EXEC GetAddressById @id={id}")
                 .AsEnumerable()
                 .FirstOrDefault();
        }

        public int GetAddressId(Address address)
        {
            int res = Int32.Parse(_context.Addresses.FromSqlInterpolated(
                $"EXEC GetAddressId @city={address.City}, @district={address.District}, @commune={address.Commune}, @address={address.Address1}").ToString());
            return res;
        }

        public List<Family> GetFamilyByIdUser(int id)
        {
            return _context.Families
                .FromSqlInterpolated($"EXEC GetFamilyByIdUser @id={id}")
                .AsEnumerable()
                .ToList();
        }

        public List<Studyprocess> GetStudyPByIdUser(int id)
        {
            return _context.Studyprocesses
                .FromSqlInterpolated($"EXEC GetStudyPByIdUser @id={id}")
                .AsEnumerable()
                .ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users
                .FromSqlInterpolated($"EXEC GetUserById @id={id}")
                .AsEnumerable()
                .FirstOrDefault();
        }

        public List<User> GetUserByType(string search, string type, string sort, int pageNumber, int pageSize)
        {
            int skipAmount = (pageNumber - 1) * pageSize;
            List<User> users = _context.Users.FromSqlInterpolated(
                $"EXEC GetUserByTypeV2 @search = {search}, @type={type}, @sort={sort}")
                .AsEnumerable()
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();
            return users;
        }

        public int GetUserId(User user)
        {
            throw new NotImplementedException();
        }

        public List<Workingprocess> GetWorkingPByIdUser(int id)
        {
            return _context.Workingprocesses
               .FromSqlInterpolated($"EXEC GetWorkingPByIdUser @id={id}")
               .AsEnumerable()
               .ToList();
        }

        public void MoveToTrash(int id)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC ToggleTrash @id={id}");
        }

        public void ResetPassword(int id)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC ResetPassword @id={id}");
        }

        public List<User> SearchUser(string searchInput, string sort, string type,
            int pageNumber = 1, int pageSize = 10)
        {
            List<User> users = new List<User>();
            int skipAmount = (pageNumber - 1) * pageSize;
            users = _context.Users.FromSqlInterpolated(
                $"EXEC SearchUserV2 @search = {searchInput}, @type = {type}, @sort = {sort}")
                .AsEnumerable()
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();

            return users;
        }

        public void ToggleStatus(int id)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC ToggleStatus @id={id}");
        }

        public void UpdateAddress(Address addressRA)
        {
            _context.Database
                .ExecuteSqlInterpolated($@"
                    EXEC UpdateAddress
                        @id={addressRA.Id},
                        @city={addressRA.City},
                        @district={addressRA.District},
                        @commune={addressRA.Commune},
                        @address={addressRA.Address1}
                ");
        }

        public void UpdateUser(User user)
        {
            _context.Database
                .ExecuteSqlInterpolated($@"
                    EXEC UpdateUser
                        @id={user.Id},
                        @covI={user.CoverImage},
                        @username={user.Username},
                        @password={user.Password},
                        @fullname={user.FullName},
                        @gender={user.Gender},
                        @dob={user.DateOfBirth},
                        @phonenumber={user.PhoneNumber},
                        @ethG={user.EthnicGroup},
                        @relig={user.Regilion},
                        @idcard={user.IdCard},
                        @culS={user.CulturalStandard},
                        @dateMod={user.DateModified}
                ");
        }

        public void UpdateUserNoImage(User user)
        {
           
            _context.Database
                .ExecuteSqlInterpolated($@"
                    EXEC UpdateUserNoImage
                        @id={user.Id},
                        @username={user.Username},
                        @password={user.Password},
                        @fullname={user.FullName},
                        @gender={user.Gender},
                        @dob={user.DateOfBirth},
                        @phonenumber={user.PhoneNumber},
                        @ethG={user.EthnicGroup},
                        @relig={user.Regilion},
                        @idcard={user.IdCard},
                        @culS={user.CulturalStandard},
                        @dateMod={user.DateModified}
                ");
        }
    }
}
