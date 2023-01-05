using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles
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
