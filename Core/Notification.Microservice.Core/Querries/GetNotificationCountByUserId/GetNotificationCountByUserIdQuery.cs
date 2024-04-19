using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Querries.GetNotificationCountByUserId;

public class GetNotificationCountByUserIdQuery : IRequest<int>
{
    public string UserId { get; set; }
}

public class GetNotificationCountByUserIdQueryHandler : IRequestHandler<GetNotificationCountByUserIdQuery, int>
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationCountByUserIdQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<int> Handle(GetNotificationCountByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _notificationRepository.GetNewNotificationCount(request.UserId);
        return result;
    }
}
