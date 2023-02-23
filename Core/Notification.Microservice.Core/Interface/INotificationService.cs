using System.Threading.Tasks;

namespace Notification.Microservice.Core.Interface
{
    public interface INotificationService
    {
        int GetNotificationCount(string userId);
    }
}
