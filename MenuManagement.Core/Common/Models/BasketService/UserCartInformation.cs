using MenuManagement.Core.Common.Models.Common;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MenuManagement.Core.Common.Models.BasketService
{
    public class UserCartInformation
    {
        public UserInformationModel UserInfo { get; set; }
        public List<JObject> Items { get; set; }
        public VendorDetail VendorDetails { get; set; }
    }
}
