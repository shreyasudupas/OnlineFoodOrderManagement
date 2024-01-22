using MenuManagement.SignalR.HubService.Common.Models;

namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Hub
{
    public interface IConnectionManager
    {
        void AddConnection(string userId, string role, string connectionId);

        HubUser GetUserConnection(string userId);

        List<HubUser> GetAllUsersConnections();

        void RemoveConnection(string userId, string connectionId);
    }
}
