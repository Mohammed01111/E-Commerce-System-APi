using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost("add")]
        public IActionResult AddProduct([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                OverallRating = 0
            };

            _productRepository.AddProduct(product);
            _productRepository.save();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;

            _productRepository.UpdateProduct(product);
            _productRepository.Save();

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpGet]
        public IActionResult GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string name = null, [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null)
        {
            var products = _productRepository.GetProducts(page, pageSize, name, minPrice, maxPrice);
            return Ok(products);
        }
    }
}


