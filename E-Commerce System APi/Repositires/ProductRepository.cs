using E_Commerce_System_APi.Models;
using System;

namespace E_Commerce_System_APi.Repositires
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context; // The database context to interact with the database (e.g., SQL Server)

        // Constructor that initializes the ProductRepository with the database context
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context; // Assigning the AppDbContext to the repository for database operations
        }

        // Method to retrieve a product by its unique ID
        public Product GetById(int id)
        {
            // Query the Products table to find a product with the specified PID (Product ID)
            return _context.Products.SingleOrDefault(p => p.PID == id);
            // SingleOrDefault returns a single product or null if no match is found
        }

        // Method to retrieve all products from the database
        public List<Product> GetAll()
        {
            // Return all products from the Products table as a list
            return _context.Products.ToList();
        }

        // Method to add a new product to the database
        public void AddProduct(Product product)
        {
            // Add the new product to the Products table
            _context.Products.Add(product);

            // Save changes to persist the new product in the database
            _context.SaveChanges();
        }

        // Method to retrieve a product by its unique ID (duplicate of GetById method, so could be removed)
        public Product GetProductById(int id)
        {
            // Using EF Core to query the product by its ID
            return _context.Products.SingleOrDefault(p => p.PID == id);
            // SingleOrDefault returns a single product or null if no match is found
        }

        // Method to update an existing product in the database
        public void UpdateProduct(Product product)
        {
            // Mark the product as updated in the Products table
            _context.Products.Update(product);

            // Save changes to persist the updated product information in the database
            _context.SaveChanges();
        }

        // Method to retrieve filtered products based on criteria such as name, price range, and pagination
        public List<Product> GetFilteredProducts(string name, decimal minPrice, decimal maxPrice, int page, int pageSize)
        {
            // Query the Products table with filtering and pagination
            return _context.Products
                .Where(p => (string.IsNullOrEmpty(name) || p.Name.Contains(name)) && // Filter by name if provided
                            (minPrice == 0 || p.Price >= minPrice) && // Filter by minimum price if provided
                            (maxPrice == 0 || p.Price <= maxPrice)) // Filter by maximum price if provided
                .Skip((page - 1) * pageSize) // Apply pagination (skip previous pages)
                .Take(pageSize) // Limit the number of records returned (pageSize)
                .ToList(); // Convert the query results to a list and return
        }
    }
}


