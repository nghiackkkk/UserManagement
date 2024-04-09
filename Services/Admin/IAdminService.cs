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

        bool CheckUsername(string username);

        bool CreateAddress(Address address);

        bool CreateUser(User user);

        bool CreateFamily(Family family);

        bool CreateWorkingP(Workingprocess workingprocess);

        bool CreateStudyP(Studyprocess studyprocess);

        int GetAddressId(Address address);

        int GetUserId(User user);

        User GetUserById(int id);

        Address GetAddressById(int id);

        List<Studyprocess> GetStudyPByIdUser(int id);

        List<Workingprocess> GetWorkingPByIdUser(int id);

        List<Family> GetFamilyByIdUser(int id);
        void DeleteStudyPByUserId(int idUser);
        void DeleteWorkPByUserId(int idUser);
        void DeleteSiblingsByUserId(int idUser);
        void UpdateAddress(Address addressRA);
        void UpdateUser(User user);
        void UpdateUserNoImage(User user);
    }
}
