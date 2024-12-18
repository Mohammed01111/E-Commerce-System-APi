using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;

namespace E_Commerce_System_APi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo; // Repository to interact with the data store for product operations

        // Constructor that initializes the ProductService with the required dependency (product repository)
        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo; // Assigning the product repository to the field
        }

        // Method to add a new product
        public Product AddProduct(ProductDto model)
        {
            // Validate that the product price is greater than zero
            if (model.Price <= 0)
                throw new Exception("Product price must be greater than zero."); // Throw an error if the price is invalid

            // Validate that the stock cannot be negative
            if (model.Stock < 0)
                throw new Exception("Stock cannot be negative."); // Throw an error if the stock value is negative

            // Create a new product object using the provided product details
            var product = new Product
            {
                Name = model.Name, // Set the product name
                Description = model.Description, // Set the product description
                Price = model.Price, // Set the product price
                Stock = model.Stock, // Set the product stock quantity
                OverallRating = 0 // Set the initial rating to zero
            };

            // Add the new product to the repository
            _productRepo.AddProduct(product);
            return product; // Return the created product object
        }

        // Method to retrieve a product by its ID
        public Product GetProductById(int id)
        {
            // Fetch the product by its ID from the repository
            var product = _productRepo.GetProductById(id);

            // Check if the product exists
            if (product == null)
                throw new Exception("Product not found."); // Throw an error if the product with the given ID does not exist

            return product; // Return the found product
        }

        // Method to update an existing product's details
        public Product UpdateProduct(int id, ProductDto model)
        {
            // Fetch the product by its ID from the repository
            var product = _productRepo.GetProductById(id);

            // Check if the product exists
            if (product == null)
                throw new Exception("Product not found."); // Throw an error if the product with the given ID does not exist

            // Update the product's details with the provided values
            product.Name = model.Name; // Update product name
            product.Description = model.Description; // Update product description
            product.Price = model.Price; // Update product price
            product.Stock = model.Stock; // Update product stock quantity

            // Update the product in the repository
            _productRepo.UpdateProduct(product);
            return product; // Return the updated product object
        }

        // Method to retrieve a list of filtered products based on the provided criteria
        public List<Product> GetFilteredProducts(string name, decimal minPrice, decimal maxPrice, int page, int pageSize)
        {
            // Fetch the filtered list of products from the repository, based on name, price range, pagination, etc.
            return _productRepo.GetFilteredProducts(name, minPrice, maxPrice, page, pageSize);
        }
    }
}


