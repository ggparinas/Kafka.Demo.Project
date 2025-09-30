// DEV-NOTE: This file defines an API controller that acts as the "Producer".
// Its purpose is to create and send messages to a Kafka topic whenever its endpoint is called.

using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace KafkaConsumerSvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        // DEV-NOTE: `ICapPublisher` is a service from the CAP library that makes it easy to publish messages.
        // It abstracts away the complexity of managing Kafka connections and sending data.
        private readonly ICapPublisher _capBus;

        // DEV-NOTE: This is the constructor. The .NET dependency injection system automatically provides
        // an instance of `ICapPublisher` when we create the OrderController.
        public OrderController(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        /// <summary>
        /// Creates a new order and publishes it as a message to the 'orders.new' Kafka topic.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder()
        {
            // DEV-NOTE: Here we create the message payload. It's a simple object that will be
            // automatically serialized into JSON format before being sent.
            var orderDetails = new { OrderId = Guid.NewGuid(), Amount = 123.45m };

            // DEV-NOTE: This defines the name of the Kafka "topic". A topic is like a dedicated channel
            // for a specific type of message. Our producer sends to this topic, and our consumer listens to it.
            const string topicName = "orders.new";

            // DEV-NOTE: This is the action! The `PublishAsync` method sends the `orderDetails` object
            // to the specified `topicName`. CAP handles all the background communication with Kafka for us.
            await _capBus.PublishAsync(topicName, orderDetails);

            return Ok($"Order created and message sent to topic '{topicName}'!");
        }
    }
}
