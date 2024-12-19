using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;

namespace E_Commerce_System_APi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo; 

      
        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

      
        public Product AddProduct(ProductDto model)
        {
          
            if (model.Price <= 0)
                throw new Exception("Product price must be greater than zero."); 

         
            if (model.Stock < 0)
                throw new Exception("Stock cannot be negative.");

         
            var product = new Product
            {
                Name = model.Name, 
                Description = model.Description, 
                Price = model.Price, 
                Stock = model.Stock, 
                OverallRating = 0 
            };

       
            _productRepo.AddProduct(product);
            return product;
        }

      
        public Product GetProductById(int id)
        {
         
            var product = _productRepo.GetProductById(id);

          
            if (product == null)
                throw new Exception("Product not found."); 

            return product; 
        }
        public List<Product> GetProducts()
        {
            return _productRepo.GetAll();
        }


        public Product UpdateProduct(int id, ProductDto model)
        {
         
            var product = _productRepo.GetProductById(id);

           
            if (product == null)
                throw new Exception("Product not found.");

        
            product.Name = model.Name; // Update product name
            product.Description = model.Description; // Update product description
            product.Price = model.Price; // Update product price
            product.Stock = model.Stock; // Update product stock quantity

            
            _productRepo.UpdateProduct(product);
            return product; 
        }

      
        public List<Product> GetFilteredProducts(string name, decimal minPrice, decimal maxPrice, int page, int pageSize)
        {
           
            return _productRepo.GetFilteredProducts(name, minPrice, maxPrice, page, pageSize);
        }
    }
}


