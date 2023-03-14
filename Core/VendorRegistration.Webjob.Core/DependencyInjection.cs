using Microsoft.Extensions.DependencyInjection;
using MenuManagement.Webjob.Core.Interfaces;
using MenuManagement.Webjob.Core.Services;

namespace MenuManagement.Webjob.Core
{
    public static class DependencyInjection
    {
        public static void AddVendorRegistrationCore(this IServiceCollection services)
        {
            services.AddSingleton<IProcessVendorRegistrationService, ProcessVendorRegistrationService>();
        }
    }
}
