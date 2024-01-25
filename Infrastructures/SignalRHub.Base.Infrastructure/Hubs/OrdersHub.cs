using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;
using System.Data;

namespace SignalRHub.Base.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersHub : Hub<IOrderHub>
    {
        private readonly IVendorUserManager _vendorUserManager;

        public OrdersHub(IVendorUserManager vendorUserManager)
        {
            _vendorUserManager = vendorUserManager;
        }

        public string GetConnectionId()
        {
            var claims = Context.User?.Claims;
            var userId = claims?.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userId))
            {
                var userConnectionInfo = _vendorUserManager.GetVendorUserConnection(userId);

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
            var vendorId = claims?.Where(x=>x.Type == "vendorId").Select(x=>x.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(vendorId) && claims is not null)
            {
                _vendorUserManager.AddVendorConnection(userId,Context.ConnectionId, vendorId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var claims = Context.User?.Claims;
            var userId = claims?.Where(x => x.Type == "userId").Select(x => x.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userId))
            {
                _vendorUserManager.RemoveVendorConnection(userId,Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
