using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<VendorDto, Vendor>()
                .ReverseMap();

            CreateMap<CoordinatesDto, Coordinates>()
                .ReverseMap();

            CreateMap<CategoryDto, Categories>()
                .ReverseMap();

        }
    }
}
