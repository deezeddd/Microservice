using CartAPI.Model;

namespace CartAPI.Service
{
    public interface ICartService
    {
        Task AddToCart(int productId, int userId, string productName);
        Task<IEnumerable<CartModel>> GetCartItems(int userId);
        Task<CartModel?> RemoveFromCart(int productId, int userId);
        Task<IEnumerable<CartModel>> GetAllItems();

    }

    // Cart service implementation
    public class CartService : ICartService
    {
        private readonly List<CartModel> _cartItems;

        public CartService()
        {
            _cartItems = new List<CartModel>();
        }

        public async Task AddToCart(int productId, int userId, string productName)
        {
           
           
            if(productId != null && userId!=null && productName!=null)
            {
                _cartItems.Add(new CartModel { ProductId = productId, UserId = userId, ProductName = productName });
            }

            // Log the updated cart items
            LogCartItems();
        }

        public async Task<IEnumerable<CartModel>> GetCartItems(int userId)
        {
            // Retrieve cart items for the specified user
            var userCartItems = _cartItems.Where(item => item.UserId == userId).ToList();

            // Check if any cart items are found

            return userCartItems;
        }

        public async Task<CartModel?> RemoveFromCart(int productId, int userId)
        {
            // Check if the product exists in the user's cart
            var existingCartItem = _cartItems.FirstOrDefault(item => item.UserId == userId && item.ProductId == productId);
            if (existingCartItem != null)
            {
                _cartItems.Remove(existingCartItem);
                LogCartItems();
                return existingCartItem;
            }
            else
            {
                return null;
            }

            
        }
        public async Task<IEnumerable<CartModel>> GetAllItems()
        {
            // Return all items in the cart
            return _cartItems.ToList();
        }

        private void LogCartItems()
        {
            foreach (var item in _cartItems)
            {
                Console.WriteLine($" {item.Id} Product ID: {item.ProductId}, User ID: {item.UserId}, Product Name : {item.ProductName} ");
            }
        }
    }
}
