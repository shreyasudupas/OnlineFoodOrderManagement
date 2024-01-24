using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using SignalRHub.Base.Infrastructure.Hubs;

namespace SignalRHub.Base.Infrastructure.Services;

public class NotificationService : INotificationHubService
{
    private readonly IConnectionManager _connectionManager;
    private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;

    public NotificationService(IConnectionManager connectionManager,
        IHubContext<NotificationHub, INotificationHub> notificationHub
        )
    {
        _connectionManager = connectionManager;
        _notificationHub = notificationHub;
    }

    public async Task SendNotificationToConnectedUsers(string toUserId,string role,int userNotificationCount)
    {
        var userSignalManager = _connectionManager.GetAllUsersConnections().Where(x => x.UserId == toUserId).FirstOrDefault();

        if (userSignalManager is not null)
        {
            await _notificationHub.Clients
                .Client(userSignalManager.ConnectionId)
                .SendUserNotification(userNotificationCount);
        }
    }
}

