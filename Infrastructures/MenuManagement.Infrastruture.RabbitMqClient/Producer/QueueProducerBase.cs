using MenuManagement.MessagingQueue.Core.Interfaces.Producers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MenuManagement.Infrastruture.RabbitMqClient.Producer
{
    public class QueueProducerBase : IQueueProducerBase, IDisposable
    {
        protected IConnection rabbitMqConnection;
        protected IModel _channel;
        private readonly ILogger _logger;

        public QueueProducerBase(ILogger<QueueProducerBase> logger)
        {
            _logger = logger;
        }

        public void InitilizeProducerQueue<TData>(string queueName, bool durable, bool exclusive, bool autodelete, TData payload
            , string routingKey, IDictionary<string, object>? arguments = null)

        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            try
            {
                rabbitMqConnection = factory.CreateConnection();
                _channel = rabbitMqConnection.CreateModel();

                _channel.QueueDeclare(queueName, durable: true,
                     exclusive: exclusive,
                     autoDelete: autodelete,
                     arguments: null);

                var json = JsonConvert.SerializeObject(payload);
                var body = Encoding.UTF8.GetBytes(json);

                _logger.LogInformation($"Publishing payload: {JsonConvert.SerializeObject(payload)}, to the queue: {queueName}");

                _channel.BasicPublish(exchange: "", routingKey: routingKey, body: body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error publishing the message in the queue {queueName}");
            }
        }

        public void Dispose()
        {
            if (_channel != null)
                _channel.Dispose();

            if (rabbitMqConnection != null)
                rabbitMqConnection.Dispose();
        }
    }
}
