using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace E_Commerce_System_APi.Models
{
    public class Product
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Required
        public string Description { get; set; }  // Optional
        public decimal Price { get; set; }  // Required, must be > 0
        public int Stock { get; set; }  // Required, must be >= 0
        public decimal OverallRating { get; set; }  // Calculated based on reviews

        // Relationships
        public ICollection<Review> Reviews { get; set; }  // A product can have many reviews
        public ICollection<OrderProduct> OrderProducts { get; set; }  // Relationship with Order via OrderProduct
    }
}
