using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Services
{
    public interface IUserService
    {
        string Login(LoginDto model);
        User Register(RegisterDto model);
        User UpdateUser(UpdateUserDto model);
    }
}