// DEV-NOTE: This file defines an API endpoint that creates and sends messages to Kafka.
// It acts as the "Producer" in the Kafka world.

using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace KafkaWithCapExample.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase // We use a controller to trigger message sending on-demand -Gab
    {
        // We need ICapPublisher because this is our producer. The producer is responsible for sending messages to a topic. -Gab
        // DEV-NOTE: `ICapPublisher` is a service provided by the CAP library that simplifies publishing messages.
        // We don't have to manage Kafka connections ourselves; we just use this interface.
        private readonly ICapPublisher _capBus; // We'll call our producer _capBus for now. -Gab

        // Inject the ICapPublisher to send messages
        // DEV-NOTE: This is a constructor that uses "Dependency Injection" (DI).
        // The application's service container automatically provides an instance of `ICapPublisher`.
        public OrderController(ICapPublisher capBus)
        {
            _capBus = capBus; // We initialize _capBus here via DI. But we don't necessarily have to do it like this. We can also do instantiation, etc... -Gab
        }

        /// <summary>
        /// Creates a new order and publishes an event to the 'orders.new' Kafka topic.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder()
        {
            // DEV-NOTE: This creates a simple object to represent the message payload.
            // It will be serialized to JSON before being sent.
            var orderDetails = new { OrderId = Guid.NewGuid(), Amount = 123.45m };

            // This is our topic name for Kafka. A producer and a consumer can only communicate with one another if they're in the same topic. -Gab
            // DEV-NOTE: A "topic" is like a channel or category in Kafka. The producer sends messages to this topic,
            // and consumers listen to this topic to receive them.
            const string topicName = "orders.new";

            // PublishAsync is what we use to send or "produce" a message to our kafka topic. -Gab
            // DEV-NOTE: This is the key line. It sends the `orderDetails` object to the `orders.new` topic.
            // CAP handles the background work of connecting to Kafka and ensuring the message is delivered.
            await _capBus.PublishAsync(topicName, orderDetails); // PublishAsync is what we use to send or "produce" a message to our kafka topic. -Gab

            return Ok($"Order created and message sent to topic '{topicName}'!");
        }
    }
}
