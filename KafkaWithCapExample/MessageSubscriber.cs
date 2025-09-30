// DEV-NOTE: This file defines the "Consumer" or "Subscriber". Its job is to listen for messages
// from a specific Kafka topic and then execute some code to process them.

using DotNetCore.CAP;
using System.Text.Json;

namespace KafkaWithCapExample
{
    // DEV-NOTE: Implementing `ICapSubscribe` is a way to mark this class as a container for message handlers.
    // CAP will automatically scan the application for classes that implement this interface.
    public class MessageSubscriber : ICapSubscribe
    {
        // DEV-NOTE: The `[CapSubscribe]` attribute is the magic that links this method to a Kafka topic.
        // It tells CAP: "When a message arrives on the 'orders.new' topic, execute this `HandleNewOrder` method."
        // The topic name here *must* match the topic name used in the `OrderController`.
        [CapSubscribe("orders.new")]
        public void HandleNewOrder(object orderDetails)
        {
            // DEV-NOTE: This configures the JSON serializer to format the output nicely (with indentation).
            // It makes the console output easier to read during the demo.
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // DEV-NOTE: This is where we would put our business logic for processing the order.
            // For this demo, we are simply printing the received message to the console to prove it worked.
            Console.WriteLine("!!!Message Received!!!");
            Console.WriteLine(JsonSerializer.Serialize(orderDetails, options));
        }
    }
}
