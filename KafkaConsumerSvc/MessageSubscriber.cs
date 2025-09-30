// DEV-NOTE: This file defines the "Consumer" (also known as a "Subscriber").
// Its role is to listen for messages on a specific Kafka topic and execute code to process them.

using DotNetCore.CAP;
using System.Text.Json;

namespace KafkaConsumerSvc
{
    // DEV-NOTE: Implementing `ICapSubscribe` tells the CAP library that this class contains
    // one or more message handler methods. CAP will automatically discover them.
    public class MessageSubscriber : ICapSubscribe
    {
        // DEV-NOTE: The `[CapSubscribe]` attribute is the key component that binds this method
        // to a Kafka topic. It tells CAP: "When a new message appears on the 'orders.new' topic,
        // run this `HandleNewOrder` method." The topic name must exactly match the one in our producer.
        [CapSubscribe("orders.new")]
        public void HandleNewOrder(object orderDetails)
        {
            // DEV-NOTE: These options are used to format the JSON output to be more readable in the console.
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // DEV-NOTE: This is the core logic for handling the received message. In a real application,
            // this is where we would update a database, send an email, or perform other business tasks.
            // For the demo, we simply print the message to the console to confirm it was received successfully.
            Console.WriteLine("!!!Message Received!!!");
            Console.WriteLine(JsonSerializer.Serialize(orderDetails, options));
        }
    }
}
