using MenuManagement.SignalR.HubService.Common.Models;

namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Services
{
    public interface INotificationUserManager
    {
        void AddConnection(string userId, string role, string connectionId);

        NotificationHubUser GetUserConnection(string userId);

        List<NotificationHubUser> GetAllUsersConnections();

        void RemoveConnection(string userId, string connectionId);
    }
}
