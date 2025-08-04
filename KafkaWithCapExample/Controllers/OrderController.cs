using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace KafkaWithCapExample.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase // We use a controller to trigger message sending on-demand -Gab
    {
        // We need ICapPublisher because this is our producer. The producer is responsible for sending messages to a topic. -Gab
        private readonly ICapPublisher _capBus; // We'll call our producer _capBus for now. -Gab

        // Inject the ICapPublisher to send messages
        public OrderController(ICapPublisher capBus)
        {
            _capBus = capBus; // We initialize _capBus here via DI. But you don't necessarily have to do it like this. You can also do instantiation, etc... -Gab
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder()
        {
            var orderDetails = new { OrderId = Guid.NewGuid(), Amount = 123.45m };

            // This is our topic name for Kafka. A producer and a consumer can only communicate with one another if they're in the same topic. -Gab
            const string topicName = "orders.new";
            await _capBus.PublishAsync(topicName, orderDetails); // PublishAsync is what we use to send or "produce" a message to our kafka topic. -Gab

            return Ok($"Order created and message sent to topic '{topicName}'!");
        }
    }
}
