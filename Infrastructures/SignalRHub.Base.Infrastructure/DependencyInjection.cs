using Microsoft.Extensions.DependencyInjection;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using SignalRHub.Base.Infrastructure.NotificationFactory.FactoryMethod;
using SignalRHub.Base.Infrastructure.NotificationFactory.Manager;
using SignalRHub.Base.Infrastructure.Services;

namespace SignalRHub.Base.Infrastructure;

public static class DependencyInjection
{
    public static void AddSignalRInfrastructure(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddTransient<INotificationHubService, NotificationService>();
        services.AddTransient<ISendAllUserNotification, SendAllUserNotification>();
        services.AddTransient<ISingleUserNotificaton,SingleUserNotification>();
        services.AddScoped<INotificationFactory, NotificationFactory.FactoryMethod.NotificationFactory>();
    }
}
