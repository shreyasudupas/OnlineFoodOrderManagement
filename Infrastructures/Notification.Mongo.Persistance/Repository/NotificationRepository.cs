using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Logging;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Mongo.Persistance.Repository
{
    public class NotificationRepository : BaseRepository<Notifications>, INotificationRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public NotificationRepository(IMongoDBContext mongoDBContext,
            IMapper mapper,
            ILogger<NotificationRepository> logger) : base(mongoDBContext)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Notifications>> GetAllNotificationByUserId(string userId,Pagination pagination)
        {
            var result = await GetAllItemsByPaginationWithFilter(n => n.UserId == userId, n=>n.RecordedTimeStamp,false , pagination);
            return result?.ToList();
        }
    }
}
