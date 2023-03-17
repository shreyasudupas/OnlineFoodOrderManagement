using MenuManagement.Infrastruture.RabbitMqClient.Consumer;
using Microsoft.Extensions.DependencyInjection;
using MenuMangement.RabbitMqClient.Domain.Interfaces;
using MenuManagement.MessagingQueue.Core.Interfaces.Producers;
using MenuManagement.Infrastruture.RabbitMqClient.Producer;

namespace MenuManagement.Infrastruture.RabbitMqClient
{
    public static class DependencyInjection
    {
        public static void AddRabbitMQInfrastruture(this IServiceCollection services)
        {
            services.AddSingleton<IVendorRegistrationConsumerServices, VendorRegistrationConsumerService>();

            services.AddTransient<IQueueProducerBase, QueueProducerBase>();
        }
    }
}
