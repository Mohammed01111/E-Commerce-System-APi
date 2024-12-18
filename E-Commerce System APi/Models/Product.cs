using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System_APi.Models
{
    public class Product
    {
        [Key]
        public int PID { get; set; }  // Primary Key

        [Required]
        public string Name { get; set; }  // Required

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }  // Required and must be > 0

        [Required]
        public int Stock { get; set; }  // Required and must be >= 0

        public decimal OverallRating { get; set; }  // Calculated from reviews

        // Relationships
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
