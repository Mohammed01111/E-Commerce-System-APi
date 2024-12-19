using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Services
{
    public interface IProductService
    {
        Product AddProduct(ProductDto model);
        List<Product> GetFilteredProducts(string name, decimal minPrice, decimal maxPrice, int page, int pageSize);
        Product GetProductById(int id);
        Product UpdateProduct(int id, ProductDto model);

        List<Product> GetProducts();
    }
}