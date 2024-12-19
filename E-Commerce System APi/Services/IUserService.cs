using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Services
{
    public interface IUserService
    {
        User GetUser(string email, string password);
        User Register(RegisterDto model);
    }
}