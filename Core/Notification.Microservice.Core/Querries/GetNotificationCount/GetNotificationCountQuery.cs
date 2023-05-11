using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Querries.GetNotificationCount
{
    public class GetNotificationCountQuery : IRequest<int>
    {
        public string Userid { get; set; }
    }

    public class GetNotificationCountQueryHandler : IRequestHandler<GetNotificationCountQuery, int>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationCountQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<int> Handle(GetNotificationCountQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetNewNotificationCount(request.Userid);
        }
    }
}
