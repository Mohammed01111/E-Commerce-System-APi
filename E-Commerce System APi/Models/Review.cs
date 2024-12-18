using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System_APi.Models
{
    public class Review
    {
        [Key]
        public int RID { get; set; }  // Primary Key

        public int UserId { get; set; }  // Foreign Key to User

        public int ProductId { get; set; }  // Foreign Key to Product

        [Required]
        public int Rating { get; set; }  // Required, 1 to 5

        public string Comment { get; set; }  // Optional

        public DateTime ReviewDate { get; set; }

        // Relationships
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
