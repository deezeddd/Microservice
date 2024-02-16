using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Model;
using OrderAPI.Service;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly List<OrderModel> _order;
        private readonly ILogger<OrderController> _logger;
        private readonly IBusControl _bus;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IBusControl bus, IOrderService orderService)
        {
            _logger = logger;
            _bus = bus;
            _order = new List<OrderModel>();
            _orderService = orderService;
        }


        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                if (orders == null)
                {
                    return BadRequest("No Orders Available");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetOrderByUser/{userId}")]
        public async Task<IActionResult> GetOrderByUser([FromRoute] int userId)
        {
            try
            {
                var orders = await _orderService.GetOrderByUser(userId);
                if(orders == null)
                {
                    return BadRequest("No Orders Available by User");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       
       
        [HttpGet("GetOrdersByProduct/{productId}")]
        public async Task<IActionResult> GetOrdersByProduct([FromRoute] int productId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByProduct(productId);
                if (orders == null)
                {
                    return BadRequest("No Orders Available by ProductId");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
