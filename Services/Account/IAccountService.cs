using UserManagement.Models;

namespace UserManagement.Services.Account
{
    public interface IAccountService
    {
        bool SaveRegisterUser(User user);

        List<string> GetAllUsername();

        bool CheckUsername(string username);

        
    }
}
