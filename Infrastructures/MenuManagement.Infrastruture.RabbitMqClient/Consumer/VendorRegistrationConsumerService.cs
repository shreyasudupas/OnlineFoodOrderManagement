using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MenuMangement.RabbitMqClient.Domain.Interfaces;
using MenuManagement.MessagingQueue.Core.Interfaces.Consumers;

namespace MenuManagement.Infrastruture.RabbitMqClient.Consumer
{
    public class VendorRegistrationConsumerService : IVendorRegistrationConsumerServices , IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        protected IConnection rabbitMqConnection;
        protected IModel _channel;
        private readonly IProcessVendorRegistrationService _processVendorRegistrationService;

        public VendorRegistrationConsumerService(IConfiguration configuration, ILogger<VendorRegistrationConsumerService> logger,
            IProcessVendorRegistrationService processVendorRegistrationService)
        {
            _configuration = configuration;
            _logger = logger;
            _processVendorRegistrationService = processVendorRegistrationService;
        }

        public void GetVendorRegistrationMessageFromQueue()
        {
            string response = string.Empty;
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            rabbitMqConnection = factory.CreateConnection();
            _channel = rabbitMqConnection.CreateModel();

            //Step 1: creating dead-letter exchange and queue
            var deadLetterExchange = _configuration.GetSection("RabbitMQConfiguration:VendorRegistration:DeadLetterExchange").Value;
            _channel.ExchangeDeclare(deadLetterExchange, ExchangeType.Fanout);

            //Then create a queue and bind the exchange to the queue created
            var deadLetterQueue = _configuration.GetSection("RabbitMQConfiguration:VendorRegistration:DeadLetterQueueName").Value;
            _channel.QueueDeclare(deadLetterQueue,true,false,false,null);
            _channel.QueueBind(deadLetterQueue, deadLetterExchange, "");


            //Step2: create a main exchange exchange and queue and bind the exchange to the queue
            var mainExchangeName = _configuration.GetSection("RabbitMQConfiguration:VendorRegistration:ExchangeName").Value;
            _channel.ExchangeDeclare(mainExchangeName, ExchangeType.Fanout);

            var arguments = new Dictionary<string, object>()
            {
                { "x-dead-letter-exchange", deadLetterExchange }
            };

            var queueName = _configuration.GetSection("RabbitMQConfiguration:VendorRegistration:QueueName").Value;
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: arguments);
            _channel.QueueBind(queueName, mainExchangeName, "");

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();
                    response = Encoding.UTF8.GetString(body);

                    _logger.LogInformation($"Message received: {response}");

                    await _processVendorRegistrationService.ProcessVendorRegistration(response);

                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception consuming Error:{ex.Message}");

                    _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }

            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        }

        public void Dispose()
        {
            if(_channel != null)
                _channel.Dispose();

            if(rabbitMqConnection != null)
                rabbitMqConnection.Dispose();
        }
    }
}
