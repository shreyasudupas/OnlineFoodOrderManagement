using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MenuManagement.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
