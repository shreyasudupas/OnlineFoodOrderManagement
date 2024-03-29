﻿using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using System;

namespace MenuManagment.Mongo.Domain.Mongo.MappingProfile
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
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
