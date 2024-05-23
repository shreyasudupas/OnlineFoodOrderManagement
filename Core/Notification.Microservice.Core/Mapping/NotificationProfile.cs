using AutoMapper;
using MenuManagment.Microservice.Core.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System;

namespace Notification.Microservice.Core.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notifications, NotificationDto>()
                .ForMember(dest=>dest.CreatedDate,act=>act.MapFrom((src,dest)=>
                {
                    return src.CreatedDate.ToLocalTime().ToString();
                }))
                ;

            CreateMap<NotificationDto, Notifications>()
                .ForMember(dest => dest.CreatedDate, act => act.MapFrom((src, dest) =>
                {
                    if (!string.IsNullOrEmpty(src.CreatedDate))
                    {
                        var d = DateTime.Parse(src.CreatedDate);
                        return d;
                    }
                    return DateTime.Now;
                }))
                ;

            CreateMap<NotificationDataDto, NotificationData>()
                .ReverseMap();
        }
    }
}
