using AutoMapper;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models.Database;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles
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
