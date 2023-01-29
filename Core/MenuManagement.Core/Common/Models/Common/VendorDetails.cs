using Newtonsoft.Json;

namespace Inventory.Microservice.Core.Common.Models.Common
{
    public class VendorDetail
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
