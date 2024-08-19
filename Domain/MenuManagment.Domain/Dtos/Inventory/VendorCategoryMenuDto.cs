using System.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MenuManagment.Mongo.Domain.Dtos.Inventory;

public record VendorCategoryMenuDto
{
    [JsonIgnore]
    [JsonProperty("id")]
    public string Id { get; set; }

    [Required]
    [MaxLength(100)]
    [JsonProperty("itemName")]
    public string ItemName { get; set; }

    [JsonIgnore]
    [JsonProperty("image")]
    public ImageModelDto Image { get; set; }

    [Required]
    [Description("Can add FoodType like Eg: Vegiterian, Non Vegetarian, Eggetarian and Vegan")]
    [JsonProperty("foodType")]
    public string FoodType { get; set; }

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
