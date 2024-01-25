using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using SignalRHub.Base.Infrastructure.Hubs;

namespace SignalRHub.Base.Infrastructure.NotificationFactory.Manager
{
    public class SingleUserNotification : ISingleUserNotificaton
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;
        private readonly INotificationUserManager _connectionManager;

        public SingleUserNotification(IHubContext<NotificationHub, INotificationHub> notificationHub,
            INotificationUserManager connectionManager)
        {
            _notificationHub = notificationHub;
            _connectionManager = connectionManager;
        }

        public async Task SendSingleNotification(string toUserId,int count)
        {
            var userSignalManager = _connectionManager.GetAllUsersConnections().Where(x => x.UserId == toUserId).FirstOrDefault();

            if (userSignalManager is not null)
            {
                await _notificationHub.Clients
                    .Client(userSignalManager.ConnectionId)
                    .SendUserNotification(count);
            }
        }
    }
}
