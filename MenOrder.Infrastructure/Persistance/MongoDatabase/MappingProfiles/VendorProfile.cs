using AutoMapper;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models.Database;
using System;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<VendorDto, Vendor>()
                .ForMember(act=>act.OpenTime,opt=>opt.MapFrom((src,dest)=> {

                    var splitTime = src.OpenTime.Split(':');
                    var hours = Convert.ToInt32(splitTime[0]);
                    var min = Convert.ToInt32(splitTime[1]);
                    var sec = Convert.ToInt32(splitTime[2]);
                    var time = new TimeSpan(hours, min, sec);
                    return time;
                }))
                .ForMember(act => act.CloseTime, opt => opt.MapFrom((src, dest) => {

                    var splitTime = src.CloseTime.Split(':');
                    var hours = Convert.ToInt32(splitTime[0]);
                    var min = Convert.ToInt32(splitTime[1]);
                    var sec = Convert.ToInt32(splitTime[2]);
                    var time = new TimeSpan(hours, min, sec);
                    return time;
                }))
                ;

            CreateMap<Vendor, VendorDto>()
                .ForMember(act => act.OpenTime, opt => opt.MapFrom((src, dest) => new DateTime() + src.OpenTime))
                .ForMember(act => act.CloseTime, opt => opt.MapFrom((src, dest) => new DateTime() + src.CloseTime))
                ;

            CreateMap<CoordinatesDto, Coordinates>()
                .ReverseMap();

            CreateMap<CategoryDto, Categories>()
                .ForMember(act => act.OpenTime, opt => opt.MapFrom((src, dest) => {

                    var splitTime = src.OpenTime.Split(':');
                    var hours = Convert.ToInt32(splitTime[0]);
                    var min = Convert.ToInt32(splitTime[1]);
                    var sec = Convert.ToInt32(splitTime[2]);
                    var time = new TimeSpan(hours, min, sec);
                    return time;
                }))
                .ForMember(act => act.CloseTime, opt => opt.MapFrom((src, dest) => {

                    var splitTime = src.CloseTime.Split(':');
                    var hours = Convert.ToInt32(splitTime[0]);
                    var min = Convert.ToInt32(splitTime[1]);
                    var sec = Convert.ToInt32(splitTime[2]);
                    var time = new TimeSpan(hours, min, sec);
                    return time;
                }))
                ;

            CreateMap<Categories, CategoryDto>()
                .ForMember(act => act.OpenTime, opt => opt.MapFrom((src, dest) => new DateTime() + src.OpenTime))
                .ForMember(act => act.CloseTime, opt => opt.MapFrom((src, dest) => new DateTime() + src.CloseTime))
                ;

        }
    }
}
