using DotNetCore.CAP;
using System.Text.Json;

namespace KafkaWithCapExample
{
    public class MessageSubscriber : ICapSubscribe
    {
        [CapSubscribe("orders.new")]
        public void HandleNewOrder(object orderDetails)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Console.WriteLine("!!!Message Received!!!");
            Console.WriteLine(JsonSerializer.Serialize(orderDetails, options));
        }
    }
}
