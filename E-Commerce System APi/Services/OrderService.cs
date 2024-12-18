using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;

namespace E_Commerce_System_APi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public Order PlaceOrder(OrderDto model, int userId)
        {
            // Ensure sufficient stock and calculate total amount
            decimal totalAmount = 0;
            foreach (var item in model.Items)
            {
                var product = _productRepo.GetById(item.ProductId);
                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found.");

                if (product.Stock < item.Quantity)
                    throw new Exception($"Insufficient stock for {product.Name}.");

                totalAmount += product.Price * item.Quantity;
                product.Stock -= item.Quantity; // Reduce stock
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount
            };

            _orderRepo.AddOrder(order);

            // Add ordered products
            foreach (var item in model.Items)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.OID,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                _orderRepo.AddOrderProduct(orderProduct);
            }

            return order;
        }
    }

}


