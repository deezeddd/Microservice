using CartAPI.Model;
using CartAPI.Service;
using MassTransit;
using Microsoft.AspNetCore.Mvc;


namespace CartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {

        private readonly List<CartModel> _cart;
        private readonly ILogger<CartController> _logger;
        private readonly IBusControl _bus;
        private readonly ICartService _cartService;

        public CartController(ILogger<CartController> logger, IBusControl bus, ICartService cartService)
        {
            _logger = logger;
            _bus = bus;
            _cart = new List<CartModel>();
            _cartService = cartService;
        }
        [HttpGet("GetCartItems/{userId}")]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            try
            {
                var cartItems = await _cartService.GetCartItems(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    return NotFound("Cart items not found for the specified user.");
                }

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("RemoveFromCart/user/{userId}/product/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            try
            {
                
               var item = _cartService.RemoveFromCart(productId, userId);
                if (item.Result != null)
                {
                    return Ok("Product removed from cart successfully");
                }
                else { return BadRequest("Product or User Doesn't Exist"); }



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllCartItems")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var cartItems = await _cartService.GetAllItems();
                if (cartItems == null || !cartItems.Any())
                {
                    return NotFound("No cart items found.");
                }

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
