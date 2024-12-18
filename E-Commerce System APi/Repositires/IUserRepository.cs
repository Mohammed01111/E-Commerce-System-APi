using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Repositires
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetByEmail(string email);
        User GetById(int id);
        void UpdateUser(User user);
    }
}