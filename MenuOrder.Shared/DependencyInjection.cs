using MenuManagement.Core.Common.Services;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MenuOrder.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedInjection(this IServiceCollection services)
        {
            services.AddScoped<IProfileUser, ProfileUser>();
            return services;
        }
    }
}
