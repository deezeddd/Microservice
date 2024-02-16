using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductAPI.Model;
using SharedLibrary;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProductController : ControllerBase
    {
        private readonly IBus _bus;

        private readonly List<ProductModel> _products;
        private readonly ILogger<ProductController> _logger; 


        public ProductController(IBus bus, ILogger<ProductController> logger)
        {
            _bus = bus;
            _logger = logger;
            // Initialize the list of products
            _products = new List<ProductModel>();
        }


        // Endpoint to add a new product
        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]

        public IActionResult AddProduct(ProductModel product)
        {
            try
            {
                // Check if product already exists
                var productCheck = _products.Find(p => p.ProductId == product.ProductId);
                if (productCheck != null)
                {
                    return BadRequest("Product already exists");
                }

                // Add product to list
                _products.Add(product);
                return Ok(new { message = "Product added successfully", product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Endpoint to remove a product
        [HttpDelete("DeleteProductById/{productId}")]
        [Authorize(Roles = "Admin")]

        public IActionResult RemoveProduct(int productId)
        {
            try
            {
                // Find product in list
                var product = _products.Find(p => p.ProductId == productId);
                if (product == null)
                {
                    return NotFound("Product not found");
                }

                // Remove product from list
                _products.Remove(product);
                return Ok(new { message = "Product removed successfully", product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Endpoint to get details of a specific product
        [HttpGet("GetProductById/{productId}")]
        [Authorize(Roles = "Admin")]

        public IActionResult GetProduct(int productId)
        {
            try
            {
                // Find product in list
                var product = _products.Find(p => p.ProductId == productId);
                if (product == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Endpoint to get a list of all products
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                if (_products.Count == 0)
                {
                    return NotFound("No products available");
                }

                return Ok(_products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [Authorize(Roles = "User")]
        [HttpPost("PlaceOrder/User/{userId}/Product/{productId}")]
        public async Task<IActionResult> PlaceOrder([FromRoute] int productId, [FromRoute] int userId)
        {
            try
            {
                //Can be done using ProductId ( Searching Product via ProductId )

                var product = _products.Find(p => p.ProductId == productId);

                if(product == null)
                {
                    return BadRequest("Product or User Doesn't Exist");
                }
                // Publish message to RabbitMQ when placing Order
                var message = new OrderMessage
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UserId = userId,
                    Price = product.Price

                };
                var endpoint = await _bus.GetSendEndpoint(new Uri("queue:Order-queue"));

                await endpoint.Send<OrderMessage>(message);

                _logger.LogInformation($"Order Message published to Order-Queue RabbitMQ: ProductId={message.ProductId}, ProductName={message.ProductName}, UserId={message.UserId}, Price={message.Price}");
                
                return Ok("Product Ordered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while publishing message to RabbitMQ");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("AddtoCart/User/{userId}/Product/{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToCart([FromRoute] int productId, [FromRoute] int userId)
        {
            try
            {
                var product = _products.Find(p => p.ProductId == productId);

                if (product == null)
                {
                    return BadRequest("Product or User Doesn't Exist");
                }

                // Publish message to RabbitMQ when adding a product to the cart
                var message = new AddToCartMessage
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UserId = userId
                };

                var endpoint = await _bus.GetSendEndpoint(new Uri("queue:add-to-cart-queue"));
                await endpoint.Send<AddToCartMessage>(message);
                _logger.LogInformation($"Message published to RabbitMQ: ProductId={message.ProductId}, ProductName={message.ProductName}, UserId={userId}");
                return Ok("Product added to Cart successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while publishing message to RabbitMQ");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
