using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Notification.Mongo.Persistance
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddNotificationMongoInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
