namespace MenuManagement.MessagingQueue.Core.Interfaces.Producers
{
    public interface IQueueProducerBase
    {
        void InitilizeProducerQueue<TData>(string queueName, bool durable, bool exclusive, bool autodelete, TData payload
            , string routingKey, IDictionary<string, object>? arguments = null);
    }
}
