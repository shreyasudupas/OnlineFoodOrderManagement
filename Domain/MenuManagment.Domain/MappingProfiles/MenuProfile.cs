﻿using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;

namespace MenuManagment.Mongo.Domain.Mongo.MappingProfile
{
    public class MenuProfile: Profile
    {
        public MenuProfile()
        {
            CreateMap<VendorMenuDto, VendorsMenus>()
                .ReverseMap();
        }
    }
}
