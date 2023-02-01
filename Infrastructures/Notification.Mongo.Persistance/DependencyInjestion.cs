using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Mongo.Persistance.Repository;

namespace Notification.Mongo.Persistance
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddNotificationMongoInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            return services;
        }
    }
}
