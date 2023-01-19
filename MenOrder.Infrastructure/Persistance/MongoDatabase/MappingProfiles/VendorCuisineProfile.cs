using AutoMapper;
using MenuManagement.Core.Mongo.Dtos;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models.Database;

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
