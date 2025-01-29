using System.ComponentModel;

namespace MenuManagement.AI.Domain.Models;

public record MenuItemModel
{
    [Description("Id of Menu Item")]
    public string MenuId { get; set; } = string.Empty;

    [Description("Id of Restaurent")]
    public string RestaurentId { get; set; } = string.Empty;

    [Description("Menu Item Name")]
    public string MenuItemName { get; set; } = string.Empty;

    [Description("Food Type like Veg, Non Veg")]
    public string FoodType { get; set; } = string.Empty;

    [Description("Price in Indian Rupees or Rs")]
    public double Price { get; set; }
    public int Rating { get; set; }
}
