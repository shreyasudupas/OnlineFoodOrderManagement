using MenOrder.Infrastructure.Services;
using MenuManagement.Core.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenOrder.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfratructure(this IServiceCollection services,IConfiguration config)
        {
            //register services
            services.AddScoped<IRedisCacheBasketService, RedisCacheBasketService>();

            return services;
        }
    }
}
