using AutoMapper;
using MediatR;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Repository.Notification;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
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
        private readonly ISignalRNotificationClient _signalRNotificationClient;

        public AddNotificationCommandHandler(INotificationRepository notificationRepository,
            IMapper mapper,
            ISignalRNotificationClient signalRNotificationClient)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _signalRNotificationClient = signalRNotificationClient;
        }

        public async Task<NotificationDto> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            var mapModel = _mapper.Map<Notifications>(request.NewNotification);
            var response = await _notificationRepository.AddNotifications(mapModel);
            var mapToDto = _mapper.Map<NotificationDto>(response);

            var count = await _notificationRepository.GetNewNotificationCount(request.NewNotification.UserId);

            if(!string.IsNullOrEmpty(response.Id))
            {
                await _signalRNotificationClient.GetAsyncCall(new MenuMangement.HttpClient.Domain.Models.NotificationSignalRRequest
                {
                    NotificationCount = count+1,
                    Role = request.NewNotification.Role,
                    UserId = request.NewNotification.UserId
                }, "");
            }
            

            return mapToDto;
        }
    }
}
