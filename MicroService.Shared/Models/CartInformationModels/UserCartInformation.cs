using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Common.Utility.Models.CartInformationModels
{
    public class UserCartInformation
    {
        public UserInfo UserInfo { get; set; }
        public List<JObject> Items { get; set; }
        public VendorDetail VendorDetails { get; set; }
    }

    public class VendorDetail
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
