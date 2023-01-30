using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Notification.Microservice.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
