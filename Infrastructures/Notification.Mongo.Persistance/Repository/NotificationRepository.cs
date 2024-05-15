using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDb.Shared.Persistance.Extensions;
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
            var result = await GetAllItemsByPaginationWithFilter(n => n.ToUserId == userId, n=>n.CreatedDate,false , pagination);
            return result?.ToList();
        }

        public async Task<Notifications> AddNotifications(Notifications newNotification)
        {
            _logger.LogInformation("AddNotification started..");
            //var recordedDateTime = newNotification.RecordedTimeStamp = System.DateTime.Now;
            await CreateOneDocument(newNotification);

            var getNotification = await GetDocumentByFilter(n => n.Description == newNotification.Description && n.FromUserId == newNotification.FromUserId
                && n.CreatedDate == newNotification.CreatedDate);
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

        public async Task<bool> UpdateNotificationToAsRead(string notificationId)
        {
            _logger.LogInformation("{MethodName} started..",nameof(UpdateNotificationToAsRead));
            var filter = Builders<Notifications>.Filter.Eq(n => n.Id, notificationId);
            var update = Builders<Notifications>.Update.Set(n => n.Read, true);
            var notifcationUpdateResult = await UpdateOneDocument(filter,update);

            if(notifcationUpdateResult.IsAcknowledged)
            {
                _logger.LogInformation("{MethodName} has updated", nameof(UpdateNotificationToAsRead));

                return true;
            }else
            {
                _logger.LogError("Error in saving {MethodName} with Id:{notificationId}", nameof(UpdateNotificationToAsRead),
                    notificationId);
                return false;
            }
        }

        public async Task<Notifications> UpdateNotification(Notifications notification)
        {
            _logger.LogInformation("{methodName} update has started..",nameof(UpdateNotification));

            var filter = Builders<Notifications>.Filter.Eq(n => n.Id, notification.Id);
            var update = Builders<Notifications>.Update.ApplyMultiFields(notification);
            var notifcationUpdateResult = await UpdateOneDocument(filter, update);

            if (notifcationUpdateResult.IsAcknowledged)
            {
                _logger.LogInformation("{methodName} has updated", nameof(UpdateNotification));

                return notification;
            }
            else
            {
                _logger.LogError("Error in saving {NotificationMethod} with Id: {notification.Id}",nameof(UpdateNotification)
                    ,notification.Id);
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
            var response = await ListDocumentsByFilter(n => n.ToUserId == userId && n.Read == false);
            var getCount = response.Count;
            return getCount;
        }

        public async Task<bool> DeleteNotification(string Id)
        {
            var filter = Builders<Notifications>.Filter.Eq(x => x.Id, Id);
            var result = await DeleteOneDocument(filter);

            if (result.IsAcknowledged)
            {
                return true;
            }
            else
            {
                _logger.LogError("Error Occured in Delteing the Notification with Id: {Id}",Id);
                return false;
            }
        }
    }
}
