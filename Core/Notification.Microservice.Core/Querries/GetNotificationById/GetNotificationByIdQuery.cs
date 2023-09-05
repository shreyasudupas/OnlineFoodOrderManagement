using AutoMapper;
using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.Microservice.Core.Querries.GetNotificationById
{
    public class GetNotificationByIdQuery : IRequest<NotificationDto>
    {
        public string Id { get; set; }
    }

    public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetNotificationByIdQueryHandler(INotificationRepository notificationRepository,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<NotificationDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            var notificationResult = await _notificationRepository.GetNotificationBasedOnId(request.Id);
            
            var mapToDto = _mapper.Map<NotificationDto>(notificationResult);

            return mapToDto;
        }
    }
}
