using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification
{
    public interface INotificationRepository
    {
        Task<List<Notifications>> GetAllNotificationByUserId(string userId, Pagination pagination);
        Task<Notifications> AddNotifications(Notifications newNotification);
        Task<Notifications> UpdateNotificationToAsRead(Notifications updateNotification);
        Task<int> GetNewNotificationCount(string userId);
    }
}
