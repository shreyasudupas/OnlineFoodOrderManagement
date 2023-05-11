using MenuManagement.MessagingQueue.Core.Interfaces.Producers;
using MenuManagement.MessagingQueue.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MenuManagement.MessagingQueue.Core.Services.Producers
{
    public class VendorRegistrationProducerService : IVendorRegistrationProducerService
    {
        private readonly ILogger _logger;
        private readonly IQueueProducerBase _queueProducerBase;
        private readonly IConfiguration _configuration;
        public VendorRegistrationProducerService(ILogger<VendorRegistrationProducerService> logger,
            IQueueProducerBase queueProducerBase,
            IConfiguration configuration)
        {
            _logger = logger;
            _queueProducerBase = queueProducerBase;
            _configuration = configuration;
        }

        public void PublishVendorInformationToQueue(VendorModel vendorModel)
        {
            //var queueName = _configuration.GetSection("RabbitMQConfiguration:VendorRegistration:QueueName").Value;
            var exchangeName = _configuration.GetSection("RabbitMQConfiguration:VendorRegistration:ExchangeName").Value;
            _queueProducerBase.InitilizeProducerQueue(exchangeName, true, false, false, vendorModel,routingKey: "");
        }
    }
}
