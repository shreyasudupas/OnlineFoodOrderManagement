using AutoMapper;
using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using MenuManagment.Mongo.Domain.Mongo.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Querries.GetAllNotificationByUserId
{
    public class GetAllNotificationByUserIdQuery : IRequest<List<NotificationDto>>
    {
        public string UserId { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }

    public class GetAllNotificationByUserIdQueryHandler : IRequestHandler<GetAllNotificationByUserIdQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IMapper mapper;

        public GetAllNotificationByUserIdQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
        }

        public async Task<List<NotificationDto>> Handle(GetAllNotificationByUserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await notificationRepository.GetAllNotificationByUserId(request.UserId,new Pagination { Skip = request.Skip , Limit = request.Limit });
            if (result != null)
            {
                var mapToDto = mapper.Map<List<NotificationDto>>(result);
                return mapToDto;
            }
            else
                return null;
        }
    }
}
