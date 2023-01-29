using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;

namespace MenuManagment.Mongo.Domain.Mongo.MappingProfile
{
    public class VendorFoodTypeProfile : Profile
    {
        public VendorFoodTypeProfile()
        {
            CreateMap<VendorFoodType, VendorFoodTypeDto>()
                .ReverseMap();
        }
    }
}
