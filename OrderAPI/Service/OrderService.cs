//using Microsoft.AspNetCore.Mvc;
using OrderAPI.Model;

namespace OrderAPI.Service
{
    public interface IOrderService
    {
        Task PlaceOrder(int productId, int userId, string productName, float price);
        Task<IEnumerable<OrderModel>> GetAllOrders();
        Task<IEnumerable<OrderModel>> GetOrderByUser(int userId);
        Task<IEnumerable<OrderModel>>  GetOrdersByProduct(int productId);
    }

    public class OrderService:IOrderService
    {
        private readonly List<OrderModel> _order;

        public OrderService()
        {
            _order = new List<OrderModel>();
        }

        public async Task PlaceOrder(int productId, int userId, string productName, float price)
        {
            _order.Add(new OrderModel { ProductId = productId, UserId = userId, ProductName = productName });

            foreach (var item in _order)
            {
                Console.WriteLine($" {item.Id} Product ID: {item.ProductId}, User ID: {item.UserId}, Product Name : {item.ProductName}, Price : {item.Price}  ");
            }

        }
        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            LogCartItems();
            return _order;
        }
        public async Task<IEnumerable<OrderModel>> GetOrderByUser(int userId)
        {
            return _order.Where(item => item.UserId == userId);
        }
        public async Task<IEnumerable<OrderModel>> GetOrdersByProduct(int productId)
        {
          
            return _order.Where(item => item.ProductId == productId);

        }
        private void LogCartItems()
        {
            foreach (var item in _order)
            {
                Console.WriteLine($" {item.Id} Product ID: {item.ProductId}, User ID: {item.UserId}, Product Name : {item.ProductName}, Price: {item.Price} ");
            }
        }
    }


}
