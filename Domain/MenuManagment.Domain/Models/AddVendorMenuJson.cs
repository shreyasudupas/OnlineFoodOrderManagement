using Newtonsoft.Json;

namespace MenuManagment.Mongo.Domain.Models
{
    public class AddVendorMenuJson
    {

        [JsonProperty("itemName", Required = Required.Always)]
        public string ItemName { get; set; }

        [JsonProperty("foodType", Required = Required.Always)]
        public string FoodType { get; set; }

        [JsonProperty("category", Required = Required.Always)]
        public string Category { get; set; }

        [JsonProperty("price", Required = Required.Always)]
        public double Price { get; set; }

        [JsonProperty("discount", Required = Required.Always)]
        public int Discount { get; set; }

        [JsonProperty("active", Required = Required.Always)]
        public bool Active { get; set; }
    }
}
