namespace E_Commerce_System_APi.Models
{
    public class Review
    {
        public int Id { get; set; }  // Primary Key
        public int UserId { get; set; }  // Foreign Key to User
        public int ProductId { get; set; }  // Foreign Key to Product
        public int Rating { get; set; }  // Required, must be 1 to 5
        public string Comment { get; set; }  // Optional
        public DateTime ReviewDate { get; set; }

        // Relationships
        public User User { get; set; }  // A review is written by one user
        public Product Product { get; set; }  // A review is linked to one product
    }
}
