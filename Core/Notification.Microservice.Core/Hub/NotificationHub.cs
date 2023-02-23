using MenuManagment.Mongo.Domain.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Hub
{
    [Authorize]
    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public string GetConnectionId() => Context.ConnectionId;

        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Claims.Where(x => x.Type == "userId").Select(x=>x.Value).FirstOrDefault();

            _connections.Add(name, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string name = Context.User.Claims.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();

            _connections.Remove(name, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
