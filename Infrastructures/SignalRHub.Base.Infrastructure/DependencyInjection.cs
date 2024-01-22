using Microsoft.Extensions.DependencyInjection;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using SignalRHub.Base.Infrastructure.Services;

namespace SignalRHub.Base.Infrastructure;

public static class DependencyInjection
{
    public static void AddSignalRInfrastructure(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddTransient<INotificationHubService, NotificationService>();
    }
}
