using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;

namespace IdenitityServer.Core.Domain.DBModel
{
    public class UserProfileAddress
    {
        public long Id { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }

        public string CityId { get; set; }
        public List<DropdownModel> MyCities { get; set; }
        public string Area { get; set; }

        public string AreaId { get; set; }
        public List<DropdownModel> MyAreas { get; set; }
        public string State { get; set; }
        
        public string StateId { get; set; }

        public List<DropdownModel> MyStates { get; set; }
        public bool IsActive { get; set; }

        //These are used only by UserType is Vendor
        public string VendorId { get; set; }

        //These are used only by UserType is Vendor
        public bool Editable { get; set; }

        public decimal? Latitude  { get; set; }

        public decimal? Longitude { get; set; }
    }

    
}
