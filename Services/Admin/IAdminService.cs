using UserManagement.Models;

namespace UserManagement.Services.Admin
{
    public interface IAdminService
    {
        List<User> GetUserByType(string search, string type, string sort, int pageNumber, int pageSize);

        void ToggleStatus(int id);

        List<User> SearchUser(string searchInput, string sortType, 
            string type, int pageNumber = 1, int pageSize = 10);

        void MoveToTrash(int id);

        void ResetPassword(int id);

        void DeletePernament(int id);
    }
}
