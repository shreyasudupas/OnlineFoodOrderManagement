using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;

namespace MenuManagment.Mongo.Domain.Mongo.MappingProfile
{
    public class MenuImageProfile : Profile
    {
        public MenuImageProfile()
        {
            CreateMap<MenuImages, MenuImageDto>()
                .ReverseMap();
        }
    }
}
