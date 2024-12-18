using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Services
{
    public interface IOrderService
    {
        Order PlaceOrder(OrderDto model, int userId);
    }
}