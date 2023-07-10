using AutoMapper;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;

namespace MenuManagment.Mongo.Domain.MappingProfiles.OrderManagement
{
    public class CartInformationProfile : Profile
    {
        public CartInformationProfile()
        {
            CreateMap<CartInformation,CartInformationDto>()
                .ReverseMap();

            CreateMap<MenuItem,MenuItemDto>()
                .ReverseMap();
        }
    }
}
