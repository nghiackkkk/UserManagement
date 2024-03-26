using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManagement2Context _context;

        public AccountService(UserManagement2Context context)
        {
            _context = context;
        }

        public bool CheckUsername(string username)
        {
            if (username != null)
            {
                var usernames = GetAllUsername();
                foreach (var name in usernames)
                {
                    if (name == username)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<String> GetAllUsername()
        {
            var usernames = _context.Users.Select(u => u.Username).ToList();
            return usernames;
        }

        //Save registered user
        public bool SaveRegisterUser(User user)
        {

            var save = _context.Database.ExecuteSqlInterpolated(
                $@"EXEC SaveRegisterUser @full_name ={user.FullName}, @username={user.Username}, @password={user.Password}, @date_created={user.DateCreated}, @date_modified={user.DateModified}");
            return true;
        }
    }
}
