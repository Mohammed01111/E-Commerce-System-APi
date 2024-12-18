using E_Commerce_System_APi.Models;
using System;

namespace E_Commerce_System_APi.Repositires
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order GetById(int id)
        {
            return _context.Orders.SingleOrDefault(o => o.OID == id);
        }

        public List<Order> GetByUserId(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void AddOrderProduct(OrderProduct orderProduct)
        {
            _context.OrderProducts.Add(orderProduct);
            _context.SaveChanges();
        }
    }
}


