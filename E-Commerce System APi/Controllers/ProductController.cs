﻿using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;
using E_Commerce_System_APi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_APi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto model)
        {
            try
            {
                var product = _productService.AddProduct(model);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult GetProducts([FromQuery] ProductFilterDto filter)
        {
            try
            {
               
                var products = _productService.GetFilteredProducts(filter.Name);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                products = _productService.GetProducts();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(products);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto model)
        {
            try
            {
                // Ensure the product exists before updating
                var existingProduct = _productService.GetProductById(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                var updatedProduct = _productService.UpdateProduct(id, model);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
