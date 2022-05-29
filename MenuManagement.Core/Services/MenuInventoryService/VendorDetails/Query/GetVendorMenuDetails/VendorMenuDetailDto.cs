using AutoMapper;
using MenuManagement.Core.Common.Mapping;
using MenuManagment.Domain.Mongo.Entities;
using System.Collections.Generic;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorMenuDetails
{
    public class VendorMenuDetailDto : IMapFrom<VendorMenuDetail>
    {
        public List<MenuColumnDetailDto> MenuColumnDetail { get; set; }
        public List<object> Data { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MenuColumnDetail, MenuColumnDetailDto>()
                .ForMember(dest=>dest.Display,act=>act.MapFrom(src=>src.DisplayOnScreen));
        }
    }
}
