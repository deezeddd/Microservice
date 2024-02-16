using CartAPI.Service;
using MassTransit;
using SharedLibrary;

namespace CartAPI
{


    public class AddToCartConsumer : IConsumer<AddToCartMessage>
    {
        private readonly ILogger<AddToCartConsumer> _logger;
        private readonly ICartService _cartService;


        public AddToCartConsumer(ILogger<AddToCartConsumer> logger, ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        public async Task Consume(ConsumeContext<AddToCartMessage> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Received message from queue: ProductId={message.ProductId}, ProductName={message.ProductName}, UserId={message.UserId}");

            await _cartService.AddToCart(message.ProductId, message.UserId, message.ProductName);

        }
    }



}
