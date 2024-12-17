namespace E_Commerce_System_APi.Models
{
    public class Order
    {
        public int Id { get; set; }  // Primary Key
        public int UserId { get; set; }  // Foreign Key to User
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }  // Calculated based on the products in the order

        // Relationships
        public User User { get; set; }  // A user places many orders
        public ICollection<OrderProduct> OrderProducts { get; set; }  // Many-to-many relationship with Product via OrderProduct
    }
}
