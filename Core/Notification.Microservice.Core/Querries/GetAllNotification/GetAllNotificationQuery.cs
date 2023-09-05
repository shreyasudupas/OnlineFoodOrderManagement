using AutoMapper;
using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Querries.GetAllNotification
{
    public class GetAllNotificationQuery : IRequest<List<NotificationDto>>
    {
    }

    public class GetAllNotificationQueryHandler : IRequestHandler<GetAllNotificationQuery,List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetAllNotificationQueryHandler(INotificationRepository notificationRepository,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<List<NotificationDto>> Handle(GetAllNotificationQuery request,CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetAllNotifications();
            var mapToDtos = _mapper.Map<List<NotificationDto>>(notifications);

            return mapToDtos;
        }
    }

}
