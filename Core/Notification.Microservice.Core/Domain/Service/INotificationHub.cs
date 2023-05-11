using System.Threading.Tasks;

namespace Notification.Microservice.Core.Domain.Service
{
    public interface INotificationHub
    {
        Task SendUserNotification(int count);
    }
}
