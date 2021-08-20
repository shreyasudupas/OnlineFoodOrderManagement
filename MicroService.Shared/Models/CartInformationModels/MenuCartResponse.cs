using System.Collections.Generic;

namespace MicroService.Shared.Models.CartInformationModels
{
    public class MenuCartResponse
    {
        public UserInfo UserInfo { get; set; }
        public object Items { get; set; }
        public VendorDetail VendorDetails { get; set; }
    }
}
