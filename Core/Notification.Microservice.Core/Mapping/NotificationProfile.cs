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
                .ForMember(dest=>dest.RecordedTimeStamp,act=>act.MapFrom((src,dest)=>
                {
                    return src.RecordedTimeStamp.ToString();
                }))
                ;

            CreateMap<NotificationDto, Notifications>()
                .ForMember(dest => dest.RecordedTimeStamp, act => act.MapFrom((src, dest) =>
                {
                    if (string.IsNullOrEmpty(src.RecordedTimeStamp))
                        return DateTime.Now;
                    else
                    {
                        var d = DateTime.Parse(src.RecordedTimeStamp);
                        return d;
                    }
                }))
                ;
        }
    }
}
