using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace KafkaConsumerSvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ICapPublisher _capBus;

        public OrderController(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder()
        {
            var orderDetails = new { OrderId = Guid.NewGuid(), Amount = 123.45m };
            const string topicName = "orders.new";
            await _capBus.PublishAsync(topicName, orderDetails);

            return Ok($"Order created and message sent to topic '{topicName}'!");
        }
    }
}
