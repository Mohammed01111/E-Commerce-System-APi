using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace E_Commerce_System_APi.Models
{
    public class User
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Required
        public string Email { get; set; }  // Unique, regex validation
        public string Password { get; set; }  // Regex validation
        public string Phone { get; set; }  // Required
        public string Role { get; set; }  // Required (e.g., "Admin", "Customer")
        public DateTime CreatedAt { get; set; }

        // Relationships
        public ICollection<Order> Orders { get; set; }  // A user can place many orders
        public ICollection<Review> Reviews { get; set; }  // A user can write many reviews
    }
}
