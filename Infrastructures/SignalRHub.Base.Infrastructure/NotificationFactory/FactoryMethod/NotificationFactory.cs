using MenuMangement.HttpClient.Domain.Models;
using SignalRHub.Base.Infrastructure.NotificationFactory.Manager;

namespace SignalRHub.Base.Infrastructure.NotificationFactory.FactoryMethod
{
    public class NotificationFactory : INotificationFactory
    {
        private readonly ISendAllUserNotification _sendAllUserNotification;
        private readonly ISingleUserNotificaton _singleUserNotificaton;

        public NotificationFactory(ISendAllUserNotification sendAllUserNotification,
            ISingleUserNotificaton singleUserNotificaton)
        {
            _sendAllUserNotification = sendAllUserNotification;
            _singleUserNotificaton = singleUserNotificaton;
        }

        public async Task SendNotification(NotificationSignalRRequest notificationSignalRRequest)
        {
            if (!notificationSignalRRequest.isSendAll)
            {
                await _singleUserNotificaton.SendSingleNotification(notificationSignalRRequest.ToUserId, notificationSignalRRequest.NotificationCount);
            }
            else
            {
                await _sendAllUserNotification.SendNotificationToAll(notificationSignalRRequest.FromUserId);
            }
        }
    }
}
