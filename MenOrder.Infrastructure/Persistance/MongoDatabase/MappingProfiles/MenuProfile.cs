using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;

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
