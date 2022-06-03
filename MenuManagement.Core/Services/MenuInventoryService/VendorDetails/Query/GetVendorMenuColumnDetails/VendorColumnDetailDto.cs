using AutoMapper;
using MenuManagement.Core.Common.Mapping;
using MenuManagment.Domain.Mongo.Entities;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorMenuColumnDetails
{
    public class VendorColumnDetailDto : IMapFrom<VendorColumnDetailEntity>
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public string DisplayOnScreen { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VendorColumnDetailEntity, VendorColumnDetailDto>()
                .ForMember(dest => dest.DisplayOnScreen, act => act.MapFrom(src => src.DisplayOnScreen));
        }
    }
}
