namespace SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;
public interface INotificationHub
{
    Task SendUserNotification(int notificationCount);
}
