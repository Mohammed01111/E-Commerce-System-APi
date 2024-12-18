using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_APi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            try
            {
                var user = _userService.Register(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Login")]
        public IActionResult Login(LoginDto model)
        {
            try
            {
                var token = _userService.Login(model);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UpdateUserDto model)
        {
            try
            {
                var updatedUser = _userService.UpdateUser(model);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
