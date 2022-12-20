using System.Collections.Generic;

namespace MenuManagement_IdentityServer.ApiController.Models
{
    public class LocationDropDown : LocationBase
    {
        public List<LocationBase> Items { get; set; }
    }
}
