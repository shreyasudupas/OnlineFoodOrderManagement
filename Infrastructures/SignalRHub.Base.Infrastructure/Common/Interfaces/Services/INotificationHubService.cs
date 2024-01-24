namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Services
{
    public interface INotificationHubService
    {
        Task SendNotificationToConnectedUsers(string toUserId, string role, int userNotificationCount);
    }
}
