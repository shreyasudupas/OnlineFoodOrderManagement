using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;

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
