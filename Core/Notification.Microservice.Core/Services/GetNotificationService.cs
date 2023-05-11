using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using Microsoft.Extensions.DependencyInjection;
using Notification.Microservice.Core.Interface;

namespace Notification.Microservice.Core.Services
{
    public class GetNotificationService : INotificationService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetNotificationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public int GetNotificationCount(string userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var notificationService = scope.ServiceProvider.GetService<INotificationRepository>();
                var count = notificationService.GetNewNotificationCount(userId).GetAwaiter().GetResult();
                return count;
            }
        }
    }
}
