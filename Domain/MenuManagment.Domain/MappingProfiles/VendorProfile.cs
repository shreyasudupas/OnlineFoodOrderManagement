﻿using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System;
using MenuManagment.Mongo.Domain.Dtos.Inventory;
using MenuManagment.Mongo.Domain.Entities.SubModel;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MenuManagment.Mongo.Domain.Mongo.MappingProfile
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<VendorDto, Vendor>()
                .ForMember(act=>act.OpenTime,opt=>opt.MapFrom((src,dest)=> {

                    if(src.OpenTime != null)
                    {
                        var splitTime = src.OpenTime.Split(':');
                        var hours = Convert.ToInt32(splitTime[0]);
                        var min = Convert.ToInt32(splitTime[1]);
                        var sec = Convert.ToInt32(splitTime[2]);
                        var time = new TimeSpan(hours, min, sec);
                        return time;
                    }else
                    {
                        return new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    }
                }))
                .ForMember(act => act.CloseTime, opt => opt.MapFrom((src, dest) => {
                    
                    if(src.CloseTime != null)
                    {
                        var splitTime = src.CloseTime.Split(':');
                        var hours = Convert.ToInt32(splitTime[0]);
                        var min = Convert.ToInt32(splitTime[1]);
                        var sec = Convert.ToInt32(splitTime[2]);
                        var time = new TimeSpan(hours, min, sec);
                        return time;
                    }else
                    {
                        return new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    }
                    
                }))
                .ForMember(dest => dest.Coordinates, act => act.MapFrom((src, dest) =>
                {
                    if(src.Coordinates is not null)
                    {
                        var position = GeoJson.Point(GeoJson.Position(src.Coordinates.Latitude, src.Coordinates.Longitute));
                        return position;
                    }
                    return null;
                }))
                ;

            CreateMap<Vendor, VendorDto>()
                .ForMember(act => act.OpenTime, opt => opt.MapFrom((src, dest) => new DateTime() + src.OpenTime))
                .ForMember(act => act.CloseTime, opt => opt.MapFrom((src, dest) => new DateTime() + src.CloseTime))
                .ForMember(dest => dest.Coordinates, act => act.MapFrom((src, dest) =>
                {
                    var result = new CoordinatesDto();
                    if (src.Coordinates is not null)
                    {
                        result.Latitude = src.Coordinates.Coordinates.X;
                        result.Longitute = src.Coordinates.Coordinates.Y;
                    }
                    return result;
                }))
                ;

            //CreateMap<CoordinatesDto, Coordinates>()
            //    .ReverseMap();

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

            CreateMap<ImageModelDto, ImageModel>()
                .ReverseMap();
        }
    }
}
