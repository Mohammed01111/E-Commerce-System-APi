using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_System_APi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepo,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher,
            ILogger<UserService> logger)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public User Register(RegisterDto model)
        {
            // Validate input
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Registration details cannot be null.");

            // Check for existing user
            if (_userRepo.GetByEmail(model.Email) != null)
                throw new InvalidOperationException("Email is already registered.");

            // Create new user
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Role = model.Role ?? "User", // Default role if not specified
                CreatedAt = DateTime.UtcNow
            };

            // Hash the password
            user.Password = _passwordHasher.HashPassword(user, model.Password);

            try
            {
                _userRepo.AddUser(user);
                _logger.LogInformation($"User registered successfully: {user.Email}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error registering user: {user.Email}");
                throw new ApplicationException("Failed to register user.", ex);
            }
        }

        public string Login(LoginDto model)
        {
            // Validate input
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Login details cannot be null.");

            if (string.IsNullOrWhiteSpace(model.Email))
                throw new ArgumentException("Email is required.", nameof(model.Email));

            if (string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Password is required.", nameof(model.Password));

            // Find user by email
            var user = _userRepo.GetByEmail(model.Email);
            if (user == null)
            {
                _logger.LogWarning($"Login attempt failed: User not found - {model.Email}");
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            // Verify password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning($"Login attempt failed: Invalid password - {model.Email}");
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            // Generate token
            var token = GenerateJwtToken(user);
            _logger.LogInformation($"User logged in successfully: {user.Email}");
            return token;
        }

        private string GenerateJwtToken(User user)
        {
            // Validate user
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot generate token for null user.");

            // Create claims
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UID.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
        };

            // Get secret key
            var secretKey = _configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("JWT Secret Key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configure token
            var expires = DateTime.UtcNow.AddHours(
                double.TryParse(_configuration["Jwt:ExpirationInHours"], out double hours)
                    ? hours
                    : 1
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User UpdateUser(UpdateUserDto model, int currentUserId)
        {
            // Validate input
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Update details cannot be null.");

            // Ensure the user is updating their own profile
            if (model.ID != currentUserId)
                throw new UnauthorizedAccessException("You can only update your own details.");

            // Find existing user
            var existingUser = _userRepo.GetById(model.ID);
            if (existingUser == null)
                throw new NotFoundException($"User with ID {model.ID} not found.");

            // Update user details
            existingUser.Name = model.Name ?? existingUser.Name;
            existingUser.Email = model.Email ?? existingUser.Email;
            existingUser.Phone = model.Phone ?? existingUser.Phone;
            existingUser.Role = model.Role ?? existingUser.Role;

            try
            {
                _userRepo.UpdateUser(existingUser);
                _logger.LogInformation($"User updated successfully: {existingUser.Email}");
                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user: {existingUser.Email}");
                throw new ApplicationException("Failed to update user.", ex);
            }
        }
    }

    // Custom exception for not found scenarios
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}