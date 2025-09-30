// DEV-NOTE: This file defines our "Consumer" or "Subscriber". Its job is to listen for messages
// from a specific Kafka topic and then execute code to process them when they arrive.

using DotNetCore.CAP;
using System.Text.Json;

namespace SecondaryConsumerSvc
{
    // DEV-NOTE: By implementing `ICapSubscribe`, we signal to the CAP library that this class
    // contains message handling logic. CAP will automatically scan it for subscription methods.
    public class MessageSubscriber : ICapSubscribe
    {
        // DEV-NOTE: The `[CapSubscribe]` attribute is what links this method to a Kafka topic.
        // It instructs CAP: "When a message is published to the 'orders.new' topic, execute this method."
        // The topic name must match the one our producer is using.
        [CapSubscribe("orders.new")]
        public void HandleNewOrder(object orderDetails)
        {
            // DEV-NOTE: We configure these JSON options simply to make the console output nicely formatted and easy to read.
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // DEV-NOTE: This is the business logic for our consumer. For this demo, we are just printing the
            // received message to the console to confirm that the end-to-end communication was successful.
            Console.WriteLine("!!!Message Received!!!");
            Console.WriteLine(JsonSerializer.Serialize(orderDetails, options));
        }
    }
}
