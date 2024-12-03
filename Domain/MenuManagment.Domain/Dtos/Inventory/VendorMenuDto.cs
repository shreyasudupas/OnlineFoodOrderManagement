using MenuManagment.Mongo.Domain.Entities.SubModel;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos
{
    [DisplayName("Vendor Menu")]
    [Description("Menu Item of particular Vendor")]
    public class VendorMenuDto
    {
        [JsonIgnore]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonIgnore]
        [Description("Can get this information from the url.")]
        [JsonProperty("vendorId")]
        public string VendorId { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonProperty("itemName")]
        public string ItemName { get; set; } = string.Empty;

        [JsonIgnore]
        [JsonProperty("image")]
        public ImageModel Image { get; set; }

        [Required]
        [Description("Can add FoodType like Eg: Vegiterian, Non Vegetarian, Eggetarian and Vegan")]
        [JsonProperty("foodType")]
        public string FoodType { get; set; } = string.Empty;

        [Required]
        [Description("Can add various custom category that belongs to the vendor like Eg: Breakfast, Lunch etc.")]
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; } = string.Empty;

        [Required]
        [JsonProperty("price")]
        public double Price { get; set; }

        [Required]
        [JsonProperty("discount")]
        public int Discount { get; set; }

        [JsonIgnore]
        [JsonProperty("rating")]
        public int Rating { get; set; }

        [Description("Initial adding of menu items it can be set to true")]
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
