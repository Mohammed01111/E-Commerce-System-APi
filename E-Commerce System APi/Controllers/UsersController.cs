using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_System_APi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            try
            {
                // Delegate the login logic to the service
                var token = _userService.Login(model);

                // Return the token and user ID as part of the response
                var user = _userService.GetByEmail(model.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                return Ok(new
                {
                    Token = token,
                    UserId = user.UID // Use the UID from the user object
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // The UpdateUser action is now protected by [Authorize]
        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UpdateUserDto model)
        {
            try
            {
                // Parse the user ID from claims, ensuring correct type
                if (!int.TryParse(GetCurrentUserId(), out int currentUserId))
                {
                    return Unauthorized("Invalid User ID in token.");
                }

                // Pass the current user ID to the service
                var updatedUser = _userService.UpdateUser(model, currentUserId);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        public string GenerateJwtToken(string userId, string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Helper method to extract user ID from the token claims
        private string GetCurrentUserId()
        {
            var userIdClaim = User?.Claims?.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "sub" ||
                c.Type == "userId");

            return userIdClaim?.Value;
        }
    }
}
