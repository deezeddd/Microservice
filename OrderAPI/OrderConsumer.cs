using MassTransit;
using MassTransit.Testing.MessageObservers;
//using OrderAPI.Model;
using OrderAPI.Service;
using SharedLibrary;
//using static ProductAPI.Controllers.ProductController;

namespace OrderAPI
{


    public class OrderConsumer : IConsumer<OrderMessage>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private readonly IOrderService _orderService;


        public OrderConsumer(ILogger<OrderConsumer> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;

        }

        public async Task Consume(ConsumeContext<OrderMessage> context)
        {

            var message = context.Message;

            _logger.LogInformation($"received message from order queue: productid={message.ProductId}, productname={message.ProductName}, userid={message.UserId}, price={message.Price}");

            await _orderService.PlaceOrder(message.ProductId, message.UserId, message.ProductName, message.Price);

        }
    }
}
