namespace SignalRHub.Base.Infrastructure.NotificationFactory.Manager
{
    public interface ISingleUserNotificaton
    {
        Task SendSingleNotification(string toUserId, int count);
    }
}
