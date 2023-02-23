using AutoMapper;
using MenuManagment.Mongo.Domain.Hubs;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Repositories;
using MongoDB.Driver;
using Notification.Microservice.Core.Hub;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Mongo.Persistance.Repository
{
    public class NotificationRepository : BaseRepository<Notifications>, INotificationRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;
        public NotificationRepository(IMongoDBContext mongoDBContext,
            IMapper mapper,
            ILogger<NotificationRepository> logger,
            IHubContext<NotificationHub,INotificationHub> notificationHub) : base(mongoDBContext)
        {
            _mapper = mapper;
            _logger = logger;
            _notificationHub = notificationHub;
        }

        public async Task<List<Notifications>> GetAllNotificationByUserId(string userId,Pagination pagination)
        {
            var result = await GetAllItemsByPaginationWithFilter(n => n.UserId == userId, n=>n.RecordedTimeStamp,false , pagination);
            return result?.ToList();
        }

        public async Task<Notifications> AddNotifications(Notifications newNotification,string connectionId)
        {
            _logger.LogInformation("AddNotification started..");
            var recordedDateTime = newNotification.RecordedTimeStamp = System.DateTime.Now;
            await CreateOneDocument(newNotification);

            var getNotification = await GetByFilter(n => n.Description == newNotification.Description && n.UserId == newNotification.UserId
                && n.RecordedTimeStamp == recordedDateTime);
            if(getNotification != null)
            {
                newNotification.Id = getNotification.Id;
                _logger.LogInformation("AddNotification ended");

                //call hub
                var result = await GetNewNotificationCount(newNotification.UserId);
                await _notificationHub.Clients.All.SendUserNotification(result);

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
                _logger.LogInformation("UpdateNotificationToAsRead as started..");
                return updateNotification;
            }else
            {
                _logger.LogError($"Error in saving UpdateNotificationToAsRead {updateNotification.Id}");
                return null;
            }
        }

        public async Task<int> GetNewNotificationCount(string userId)
        {
            var response = await GetListByFilter(n => n.UserId == userId && n.Read == false);
            var getCount = response.Count;
            return getCount;
        }
    }
}
