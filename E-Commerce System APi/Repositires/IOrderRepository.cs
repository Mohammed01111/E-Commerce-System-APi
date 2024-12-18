using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Repositires
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        void AddOrderProduct(OrderProduct orderProduct);
        Order GetById(int id);
        List<Order> GetByUserId(int userId);
    }
}