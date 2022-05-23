using AutoMapper;
using MenuManagement.Core.Common.Mapping;
using MenuManagment.Domain.Mongo.Entities;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorDetails
{
    public class VendorDetailsResponseDto : IMapFrom<VendorDetail>
    {
        public string VendorId { get; set; }
        public string VendorName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public int Rating { get; set; }
        
    }
    
}
