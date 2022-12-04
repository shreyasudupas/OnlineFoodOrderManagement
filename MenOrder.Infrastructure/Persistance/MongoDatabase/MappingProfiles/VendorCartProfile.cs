using AutoMapper;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using MenuManagment.Domain.Mongo.Entities;
using VendorDetail = MenuManagment.Domain.Mongo.Entities.VendorDetail;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.MappingProfiles
{
    public class VendorCartProfile : Profile
    {
        public VendorCartProfile()
        {
            CreateMap<VendorCartConfigurations, VendorCartDetails>()
                .ReverseMap();

            CreateMap<CartVendorDetail, VendorDetail>()
                .ReverseMap();

            CreateMap<ColumnDetail, MenuManagment.Domain.Mongo.Entities.ColumnDetail>()
                .ReverseMap();
        }
    }
}
