using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_System_APi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo; // Repository to interact with the data store for user operations
        private readonly IConfiguration _configuration; // Configuration to access app settings (e.g., JWT secret, issuer)

        // Constructor that initializes the UserService with the required dependencies
        public UserService(IUserRepository userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo; // Assigning the user repository to the field
            _configuration = configuration; // Assigning the configuration to the field
        }

        // Method to register a new user
        public User Register(RegisterDto model)
        {
            // Check if a user with the same email already exists
            var existingUser = _userRepo.GetByEmail(model.Email);
            if (existingUser != null)
            {
                throw new Exception("Email is already registered."); // Throw an error if the email is already used
            }

            // Create a new user object with the provided details
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone,
                Role = model.Role,
                CreatedAt = DateTime.UtcNow, // Set the user creation time to the current UTC time
            };

            // Add the new user to the repository
            _userRepo.AddUser(user);
            return user; // Return the created user object
        }

        // Method to log in an existing user
        public string Login(LoginDto model)
        {
            // Fetch the user by their email from the repository
            var user = _userRepo.GetByEmail(model.Email);

            // Check if the user exists and if the password matches
            if (user == null || model.Password != user.Password)
            {
                throw new Exception("Invalid credentials."); // Throw an error if the email or password is incorrect
            }

            // Generate and return a JWT token if login is successful
            return GenerateJwtToken(user);
        }

        // Private method to generate a JWT token for an authenticated user
        private string GenerateJwtToken(User user)
        {
            // Define the claims (user information) that will be included in the token
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UID.ToString()), // User ID as the unique identifier
            new Claim(ClaimTypes.Name, user.Name), // User's name
            new Claim(ClaimTypes.Email, user.Email), // User's email
            new Claim(ClaimTypes.Role, user.Role) // User's role (e.g., admin, user)
        };

            // Get the JWT secret key from the configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // Create the signing credentials using the secret key and HMACSHA256 algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token with claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // JWT issuer (typically your application)
                audience: _configuration["Jwt:Audience"], // JWT audience (typically who the token is intended for)
                claims: claims, // Claims to include in the token
                expires: DateTime.Now.AddHours(1), // Set token expiration to 1 hour from now
                signingCredentials: creds // Signing credentials to secure the token
            );

            // Return the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method to update an existing user's details
        public User UpdateUser(UpdateUserDto model)
        {
            // Fetch the user by their ID from the repository
            var existingUser = _userRepo.GetById(model.ID);

            // Check if the user exists
            if (existingUser == null)
            {
                throw new Exception("User not found."); // Throw an error if the user with the provided ID doesn't exist
            }

            // Update the user's details with the provided values, keeping existing values if no new ones are provided
            existingUser.Name = model.Name ?? existingUser.Name; // If Name is not provided, keep the old value
            existingUser.Email = model.Email ?? existingUser.Email; // If Email is not provided, keep the old value
            existingUser.Phone = model.Phone ?? existingUser.Phone; // If Phone is not provided, keep the old value
            existingUser.Role = model.Role ?? existingUser.Role; // If Role is not provided, keep the old value

            // Update the user in the repository with the modified details
            _userRepo.UpdateUser(existingUser);
            return existingUser; // Return the updated user object
        }
    }
}

