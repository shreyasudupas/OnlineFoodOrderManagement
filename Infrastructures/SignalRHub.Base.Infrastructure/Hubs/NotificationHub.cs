using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using System.Security.Claims;

namespace SignalRHub.Base.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly IConnectionManager connectionManager;

        public NotificationHub(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public string GetConnectionId()
        {
            var claims = Context.User?.Claims;
            var userId = claims?.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();

            if(!string.IsNullOrEmpty(userId))
            {
                var userConnectionInfo = connectionManager.GetUserConnection(userId);

                if (userConnectionInfo != null)
                {
                    return userConnectionInfo.ConnectionId;
                }
                else
                    return Context.ConnectionId;
            }

            return string.Empty;
        }

        public override async Task OnConnectedAsync()
        {
            var claims = Context.User?.Claims;
            var name = claims?.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();
            var role = claims?.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();

            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(role) && claims is not null)
                connectionManager.AddConnection(name, role, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var claims = Context.User?.Claims;
            var name = claims?.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();

            if(!string.IsNullOrEmpty(name))
                connectionManager.RemoveConnection(name, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
