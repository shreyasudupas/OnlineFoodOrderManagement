using MenuManagement.Core.Common.Models.Common;

namespace MenuManagement.Core.Common.Models.BasketService
{
    public class MenuCartResponse
    {
        public UserInformationModel UserInfo { get; set; }
        public object Items { get; set; }
        public VendorDetail VendorDetails { get; set; }
    }
}
