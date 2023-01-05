using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles
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
