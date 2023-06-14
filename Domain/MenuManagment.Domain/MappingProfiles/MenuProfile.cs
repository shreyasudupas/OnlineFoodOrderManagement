using AutoMapper;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using MenuManagment.Mongo.Domain.Dtos.Inventory;
using MenuManagment.Mongo.Domain.Entities.SubModel;

namespace MenuManagment.Mongo.Domain.Mongo.MappingProfile
{
    public class MenuProfile: Profile
    {
        public MenuProfile()
        {
            CreateMap<VendorMenuDto, VendorsMenus>()
                .ReverseMap();

            CreateMap<ImageModelDto,ImageModel>()
                .ReverseMap();
        }
    }
}
