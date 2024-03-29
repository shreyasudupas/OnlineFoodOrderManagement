﻿using AutoMapper;
using Common.Mongo.Database.Data.Models;
using MenuInventory.Microservice.Models.Vendor;
using MenuInventory.Microservice.Models.VendorCartConfiguration;

namespace MenuInventory.Microservice.Mapper
{
    public class VendorProfile:Profile
    {
        public VendorProfile()
        {
            //CreateMap<VendorCartConfigurations, VendorCartConfigurationResponse>()
            //    .ForMember(dest => dest.VendorDetails, act => act.MapFrom(source => new VendorDetail()
            //    {
            //        VendorId = source.VendorDetails.VendorId,
            //        Description = source.VendorDetails.Description,
            //        Location = source.VendorDetails.Location,
            //        Rating = source.VendorDetails.Rating,
            //        VendorName = source.VendorDetails.VendorName
            //    }));
            //.ForMember(dest=>dest.ColumnDetails,act=>act.MapFrom(source=> new ColumnDetails() 
            //{
            //    ColumnDescription = source.ColumnDetails.
            //}))
            CreateMap<CartVendorDetail, VendorListResponse>()
                .ForMember(dest=>dest.Id,act=>act.MapFrom(src=>src.VendorId));

            CreateMap<ColumnDetail, ColumnDetails>()
                .ForMember(dest=>dest.DisplayScreen,act=>act.MapFrom(src=>src.Display));
        }
    }
}
