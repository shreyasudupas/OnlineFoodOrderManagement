using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDb.Shared.Persistance.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Mongo.Persistance.Repository
{
    public class NotificationRepository : BaseRepository<Notifications>, INotificationRepository
    {
        private readonly ILogger _logger;

        public NotificationRepository(
            IOptions<MongoDatabaseConfiguration> mongoDatabaseSettings,
            ILogger<NotificationRepository> logger
            ) : base(mongoDatabaseSettings)
        {
            _logger = logger;
        }

        public async Task<List<Notifications>> GetAllNotifications()
        {
            var result = await GetAllItems();
            return result?.ToList();
        }
        public async Task<List<Notifications>> GetAllNotificationByUserId(string userId,Pagination pagination)
        {
            var result = await GetAllItemsByPaginationWithFilter(n => n.FromUserId == userId, n=>n.RecordedTimeStamp,false , pagination);
            return result?.ToList();
        }

        public async Task<Notifications> AddNotifications(Notifications newNotification)
        {
            _logger.LogInformation("AddNotification started..");
            var recordedDateTime = newNotification.RecordedTimeStamp = System.DateTime.Now;
            await CreateOneDocument(newNotification);

            var getNotification = await GetDocumentByFilter(n => n.Description == newNotification.Description && n.FromUserId == newNotification.FromUserId
                && n.RecordedTimeStamp == recordedDateTime);
            if(getNotification != null)
            {
                newNotification.Id = getNotification.Id;
                _logger.LogInformation("AddNotification ended");

                return newNotification;
            }
            else
            {
                _logger.LogInformation("Unable to get Notification in AddNew Notification");
                return newNotification;
            }
        }

        public async Task<Notifications> UpdateNotificationToAsRead(Notifications updateNotification)
        {
            _logger.LogInformation("UpdateNotificationToAsRead started..");
            var filter = Builders<Notifications>.Filter.Eq(n => n.Id,updateNotification.Id);
            var update = Builders<Notifications>.Update.Set(n => n.Read, true);
            var notifcationUpdateResult = await UpdateOneDocument(filter,update);

            if(notifcationUpdateResult.IsAcknowledged)
            {
                _logger.LogInformation("UpdateNotificationToAsRead has updated");

                return updateNotification;
            }else
            {
                _logger.LogError($"Error in saving UpdateNotificationToAsRead {updateNotification.Id}");
                return null;
            }
        }

        public async Task<Notifications> GetNotificationBasedOnId(string id)
        {
            var notification = await GetById(id);

            return notification;
        }

        public async Task<int> GetNewNotificationCount(string userId)
        {
            var response = await ListDocumentsByFilter(n => n.FromUserId == userId && n.Read == false);
            var getCount = response.Count;
            return getCount;
        }
    }
}
