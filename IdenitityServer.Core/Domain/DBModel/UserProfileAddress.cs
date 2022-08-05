using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;

namespace IdenitityServer.Core.Domain.DBModel
{
    public class UserProfileAddress
    {
        public long Id { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public List<DropdownModel> MyCities { get; set; }
        public string Area { get; set; }
        public List<DropdownModel> MyAreas { get; set; }
        public string State { get; set; }

        public List<DropdownModel> MyStates { get; set; }
        public bool IsActive { get; set; }
    }

    
}
