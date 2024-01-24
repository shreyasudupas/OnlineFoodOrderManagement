namespace SignalRHub.Base.Infrastructure.NotificationFactory.Manager
{
    public interface ISendAllUserNotification
    {
        Task SendNotificationToAll(string fromUserId);
    }
}
