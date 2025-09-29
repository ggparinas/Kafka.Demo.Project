using DotNetCore.CAP;

namespace KafkaConsumerSvc
{
    public class MessageSubscriber : ICapSubscribe // We need to use ICapSubscribe to make our consumer. -Gab
    {
        [CapSubscribe("orders.new")] // We use CapSubscribe followed by our topic name to make our consumer method. Any method that has CapSubscribe will turn it into a consumer that'll receive messages from the specified topic name. -Gab
        public void HandleNewOrder(object orderDetails) // The method is then executed everytime a message is sent to that topic. -Gab
        {
            Console.WriteLine("Hello World!"); // For example, in our case, it'll write "Hello World!" everytime a message is received in "orders.new". -Gab
            // You can put anything here. -Gab
        }
    }
}
