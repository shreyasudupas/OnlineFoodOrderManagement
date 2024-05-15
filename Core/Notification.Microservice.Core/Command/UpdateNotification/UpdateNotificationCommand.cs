using AutoMapper;
using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Command.UpdateNotification
{
    public class UpdateNotificationCommand : IRequest<NotificationDto>
    {
        public NotificationDto Notification { get; set; }
    }

    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, NotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public UpdateNotificationCommandHandler(INotificationRepository notificationRepository,
            IMapper mapper)
        {
            this._notificationRepository = notificationRepository;
            this._mapper = mapper;
        }

        public async Task<NotificationDto> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notificationModel = _mapper.Map<Notifications>(request.Notification);
            var result = await _notificationRepository.UpdateNotification(notificationModel);
            return _mapper.Map<NotificationDto>(result);
        }
    }
}
