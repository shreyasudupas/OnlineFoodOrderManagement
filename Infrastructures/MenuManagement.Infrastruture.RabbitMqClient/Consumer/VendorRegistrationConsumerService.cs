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

            var queueName = _configuration.GetSection("RabbitMQConfiguration:VendorRegistration:QueueName").Value;
            _channel.QueueDeclare(queueName, durable: true,exclusive: false,autoDelete: false,arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                response = Encoding.UTF8.GetString(body);

                _channel.BasicAck(eventArgs.DeliveryTag, false);

                _logger.LogInformation($"Message received: {response}");

                await _processVendorRegistrationService.ProcessVendorRegistration(response);
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
