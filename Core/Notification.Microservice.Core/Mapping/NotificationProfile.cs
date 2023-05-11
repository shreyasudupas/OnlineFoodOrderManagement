using AutoMapper;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;

namespace Notification.Microservice.Core.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notifications, NotificationDto>()
                .ReverseMap();
        }
    }
}
