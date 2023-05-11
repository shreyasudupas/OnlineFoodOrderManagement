using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;

namespace MenuManagment.Mongo.Domain.Mongo.MappingProfile
{
    public class VendorCuisineProfile : Profile
    {
        public VendorCuisineProfile()
        {
            CreateMap<VendorCuisineType, VendorCuisineDto>()
                .ReverseMap();
        }
    }
}
