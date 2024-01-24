using MenuMangement.HttpClient.Domain.Models;

namespace SignalRHub.Base.Infrastructure.NotificationFactory.FactoryMethod
{
    public interface INotificationFactory
    {
        Task SendNotification(NotificationSignalRRequest notificationSignalRRequest);
    }
}