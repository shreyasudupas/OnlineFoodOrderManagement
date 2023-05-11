using Microsoft.Extensions.DependencyInjection;
using MenuManagement.MessagingQueue.Core.Services.Consumers;
using MenuManagement.MessagingQueue.Core.Interfaces.Consumers;
using MenuManagement.MessagingQueue.Core.Interfaces.Producers;
using MenuManagement.MessagingQueue.Core.Services.Producers;

namespace MenuManagement.MessagingQueue.Core
{
    public static class DependencyInjection
    {
        public static void AddVendorRegistrationCore(this IServiceCollection services)
        {
            services.AddSingleton<IProcessVendorRegistrationService, ProcessVendorRegistrationService>();
            services.AddSingleton<IVendorRegistrationProducerService, VendorRegistrationProducerService>();
        }
    }
}
