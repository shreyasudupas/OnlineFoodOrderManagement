using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification
{
    public interface INotificationRepository
    {
        Task<List<Notifications>> GetAllNotifications();
        Task<List<Notifications>> GetAllNotificationByUserId(string userId, Pagination pagination);
        Task<Notifications> AddNotifications(Notifications newNotification);
        Task<bool> UpdateNotificationToAsRead(string notificationId);
        Task<int> GetNewNotificationCount(string userId);
        Task<Notifications> GetNotificationBasedOnId(string id);
        Task<bool> DeleteNotification(string Id);

        Task<Notifications> UpdateNotification(Notifications notification);
    }
}
