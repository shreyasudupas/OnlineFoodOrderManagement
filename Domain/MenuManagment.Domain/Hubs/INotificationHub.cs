using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Hubs
{
    public interface INotificationHub
    {
        Task SendUserNotification(int count);
    }
}
