using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Repositires
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        List<Product> GetAll();
        Product GetById(int id);
        List<Product> GetFilteredProducts(string name);
        Product GetProductById(int id);
        void UpdateProduct(Product product);
    }
}