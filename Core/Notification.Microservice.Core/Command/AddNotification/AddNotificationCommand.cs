using AutoMapper;
using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Command.AddNotification
{
    public class AddNotificationCommand : IRequest<NotificationDto>
    {
        public NotificationDto NewNotification { get; set; }
    }

    public class AddNotificationCommandHandler : IRequestHandler<AddNotificationCommand, NotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public AddNotificationCommandHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<NotificationDto> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            var mapModel = _mapper.Map<Notifications>(request.NewNotification);
            var response = await _notificationRepository.AddNotifications(mapModel,request.NewNotification.ConnectionId);
            var mapToDto = _mapper.Map<NotificationDto>(response);
            return mapToDto;
        }
    }
}
