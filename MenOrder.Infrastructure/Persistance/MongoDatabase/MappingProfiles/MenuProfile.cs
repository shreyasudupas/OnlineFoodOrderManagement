using AutoMapper;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models.Database;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles
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
