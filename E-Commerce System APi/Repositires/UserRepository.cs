

namespace E_Commerce_System_APi.Repositires
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context; // The database context to interact with the database (e.g., SQL Server)

        // Constructor that initializes the UserRepository with the database context
        public UserRepository(AppDbContext context)
        {
            _context = context; // Assigning the AppDbContext to the repository for database operations
        }

        // Method to get a user by their email address
        public User GetByEmail(string email)
        {
            // Query the Users table to find a user with the specified email
            return _context.Users.SingleOrDefault(u => u.Email == email);
            // SingleOrDefault returns a single user or null if no match is found
        }

        // Method to get a user by their unique ID
        public User GetById(int id)
        {
            // Query the Users table to find a user with the specified UID (unique ID)
            return _context.Users.SingleOrDefault(u => u.UID == id);
            // SingleOrDefault returns a single user or null if no match is found
        }

        // Method to add a new user to the database
        public void AddUser(User user)
        {
            // Add the new user to the Users table
            _context.Users.Add(user);

            // Save changes to persist the new user in the database
            _context.SaveChanges();
        }

        // Method to update an existing user's details in the database
        public void UpdateUser(User user)
        {
            // Mark the user as updated in the Users table
            _context.Users.Update(user);

            // Save changes to persist the updated user information in the database
            _context.SaveChanges();
        }
    }

}
