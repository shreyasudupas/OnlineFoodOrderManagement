using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Command.UpdateNotification
{
    public class UpdateNotificationReadOnlyCommand : IRequest<bool>
    {
        public string NotificationId { get; set; } = null;
    }

    public class UpdateNotificationReadOnlyCommandHandler : IRequestHandler<UpdateNotificationReadOnlyCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public UpdateNotificationReadOnlyCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(UpdateNotificationReadOnlyCommand request, CancellationToken cancellationToken)
        {
            if(!string.IsNullOrEmpty(request.NotificationId))
            {
                var result = await _notificationRepository.UpdateNotificationToAsRead(request.NotificationId);
                return result;
            } 
            else
            {
                return false;
            }
        }
    }
}
