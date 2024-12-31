using MenuManagment.Mongo.Domain.Entities.SubModel;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        [JsonProperty("categoryDetails")]
        public CategoryDetailsDto CategoryDetails { get; set; }

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

        public static List<VendorMenuDto> MaptoDto(List<VendorsMenus> vendorMenus, List<VendorCategory> categories)
        {
            List<VendorMenuDto> response = new();
            foreach (var category in categories)
            {
                var categoryMenus = vendorMenus.Where(x => x.CategoryId == category.Id);
                foreach (var vendorMenu in categoryMenus)
                {
                    response.Add(new VendorMenuDto
                    {
                        Id = vendorMenu.Id,
                        ItemName = vendorMenu.ItemName,
                        Price = vendorMenu.Price,
                        VendorId = vendorMenu.VendorId,
                        CategoryDetails = new CategoryDetailsDto
                        {
                            CategoryId = category.Id,
                            CategoryName = category.Name
                        },
                        Rating = vendorMenu.Rating,
                        Image = new ImageModel
                        {
                            ImageFileName = vendorMenu.Image.ImageFileName,
                            ImageId = vendorMenu.Image.ImageId
                        },
                        FoodType = vendorMenu.FoodType,
                        Discount = vendorMenu.Discount,
                        Active = vendorMenu.Active,
                    });
                }
            }

            return response;
        }
    }

    public record CategoryDetailsDto
    {
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; } = string.Empty;

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
