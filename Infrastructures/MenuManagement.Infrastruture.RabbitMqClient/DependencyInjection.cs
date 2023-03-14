using MenuManagement.Infrastruture.RabbitMqClient.Consumer;
using Microsoft.Extensions.DependencyInjection;
using MenuManagement.Webjob.Core.Interfaces;

namespace MenuManagement.Infrastruture.RabbitMqClient
{
    public static class DependencyInjection
    {
        public static void AddRabbitMQInfrastruture(this IServiceCollection services)
        {
            services.AddSingleton<IVendorRegistrationConsumerServices, VendorRegistrationConsumerService>();
        }
    }
}
