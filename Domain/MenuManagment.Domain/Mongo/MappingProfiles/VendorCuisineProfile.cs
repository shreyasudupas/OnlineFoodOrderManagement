using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Dtos;
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
