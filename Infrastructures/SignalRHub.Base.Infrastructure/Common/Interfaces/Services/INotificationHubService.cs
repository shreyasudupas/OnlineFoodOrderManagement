namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Services
{
    public interface INotificationHubService
    {
        Task SendNotificationToConnectedUsers(string userId, string role, int userNotificationCount);
    }
}
