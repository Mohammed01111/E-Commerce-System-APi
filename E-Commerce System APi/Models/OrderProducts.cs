using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System_APi.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }  // Foreign Key to Order

        public int ProductId { get; set; }  // Foreign Key to Product

        [Required]
        public int Quantity { get; set; }  // Required, must be > 0

        // Relationships
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
