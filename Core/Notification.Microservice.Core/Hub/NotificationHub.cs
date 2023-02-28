using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notification.Microservice.Core.Domain.Service;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Hub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly IConnectionMapping _connectionMapping;

        public NotificationHub(IConnectionMapping connectionMapping)
        {
            _connectionMapping = connectionMapping;
        }

        public string GetConnectionId()
        {
            var claims = Context.User.Claims;
            var userId = claims.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();

            var connManager = _connectionMapping.GetConnection(userId);
            if (connManager != null)
            {
                return connManager.ConnectionId;
            }
            else
                return Context.ConnectionId;
        }

        public override async Task OnConnectedAsync()
        {
            var claims = Context.User.Claims;
            var name = claims.Where(x => x.Type == "userId").Select(x=>x.Value).FirstOrDefault();
            var role = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();

            _connectionMapping.Add(name, role, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var claims = Context.User.Claims;
            var name = claims.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();

            _connectionMapping.Remove(name, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
