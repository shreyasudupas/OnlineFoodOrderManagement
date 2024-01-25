using SignalRHub.Base.Infrastructure.Common.Interfaces.Models;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Services;

namespace SignalRHub.Base.Infrastructure.Services
{
    public class VendorUserManager : IVendorUserManager
    {
        private readonly List<VendorHubUser> _vendorUsers;
        private readonly object _lockObj = new object();

        public VendorUserManager()
        {
            _vendorUsers = new();
        }

        public void AddVendorConnection(string userId, string connectionId, string vendorId)
        {
            lock (_lockObj)
            {
                var user = _vendorUsers.Find(x => x.UserId == userId);
                if (user == null)
                {
                    _vendorUsers.Add(new VendorHubUser
                    {
                        UserId = userId,
                        VendorId = vendorId,
                        ConnectionId = connectionId
                    });
                }
            }
        }

        public List<VendorHubUser> GetAllVendorUsersConnections()
        {
            return _vendorUsers;
        }

        public VendorHubUser? GetVendorUserConnection(string userId)
        {
            var user = _vendorUsers.Find(x => x.UserId == userId);
            if (user is not null)
            {
                return user;
            }

            return null;
        }

        public void RemoveVendorConnection(string userId, string connectionId)
        {
            lock (_lockObj)
            {
                var user = _vendorUsers.Find(x => x.UserId == userId && x.ConnectionId == connectionId);
                if (user != null)
                {
                    _vendorUsers.Remove(user);
                }
            }
        }
    }
}
