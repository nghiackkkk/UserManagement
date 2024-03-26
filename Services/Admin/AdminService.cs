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

        public void DeletePernament(int id)
        {
            _context.Database.ExecuteSqlInterpolated($@"EXEC DeletePernament @id = {id}");
        }

        public List<User> GetUserByType(string search, string type, string sort, int pageNumber, int pageSize)
        {
            int skipAmount = (pageNumber - 1) * pageSize;
            List<User> users = _context.Users.FromSqlInterpolated(
                $"EXEC GetUserByTypeV2 @search = {search},@type={type}, @sort={sort}")
                .AsEnumerable()
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();
            return users;
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
    }
}
