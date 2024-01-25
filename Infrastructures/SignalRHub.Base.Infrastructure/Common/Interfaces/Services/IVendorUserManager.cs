using SignalRHub.Base.Infrastructure.Common.Interfaces.Models;

namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Services
{
    public interface IVendorUserManager
    {
        void AddVendorConnection(string userId, string connectionId, string vendorId);
        List<VendorHubUser> GetAllVendorUsersConnections();
        VendorHubUser? GetVendorUserConnection(string userId);
        void RemoveVendorConnection(string userId, string connectionId);
    }
}