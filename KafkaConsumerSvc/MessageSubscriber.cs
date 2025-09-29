using DotNetCore.CAP;

namespace KafkaConsumerSvc
{
    public class MessageSubscriber : ICapSubscribe
    {
        [CapSubscribe("orders.new")]
        public void HandleNewOrder(object orderDetails)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
