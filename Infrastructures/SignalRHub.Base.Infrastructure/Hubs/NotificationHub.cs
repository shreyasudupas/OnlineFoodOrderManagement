﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using System.Security.Claims;

namespace SignalRHub.Base.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly INotificationUserManager connectionManager;

        public NotificationHub(INotificationUserManager connectionManager)
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
            var userId = claims?.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();
            var role = claims?.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();

            if(!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(role) && claims is not null)
                connectionManager.AddConnection(userId, role, Context.ConnectionId);

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
