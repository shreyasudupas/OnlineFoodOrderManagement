using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using SignalRHub.Base.Infrastructure.Hubs;

namespace SignalRHub.Base.Infrastructure.NotificationFactory.Manager
{
    public class SendAllUserNotification : ISendAllUserNotification
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;
        private readonly INotificationUserManager _connectionManager;

        public SendAllUserNotification(IHubContext<NotificationHub, INotificationHub> notificationHub,
            INotificationUserManager connectionManager)
        {
            _notificationHub = notificationHub;
            _connectionManager = connectionManager;
        }

        public async Task SendNotificationToAll(string fromUserId)
        {
            var userManager = _connectionManager.GetAllUsersConnections().Where(u => u.UserId == fromUserId).FirstOrDefault();
            
            //dont send to caller except that send to all
            if(userManager is not null)
            {
                await _notificationHub.Clients.AllExcept(userManager.ConnectionId).SendToAllNotification();
            }
        }
    }
}
