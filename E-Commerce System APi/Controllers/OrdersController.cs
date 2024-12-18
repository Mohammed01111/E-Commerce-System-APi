using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;
using E_Commerce_System_APi.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_APi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] OrderDto model)
        {
            try
            {
                var order = _orderService.PlaceOrder(model, userId: 1); // Hardcoded userId for simplicity
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


