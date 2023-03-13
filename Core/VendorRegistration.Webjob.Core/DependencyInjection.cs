using Microsoft.Extensions.DependencyInjection;
using VendorRegistration.Webjob.Core.Interfaces;
using VendorRegistration.Webjob.Core.Services;

namespace VendorRegistration.Webjob.Core
{
    public static class DependencyInjection
    {
        public static void AddVendorRegistrationCore(this IServiceCollection services)
        {
            services.AddSingleton<IProcessVendorRegistrationService, ProcessVendorRegistrationService>();
        }
    }
}
