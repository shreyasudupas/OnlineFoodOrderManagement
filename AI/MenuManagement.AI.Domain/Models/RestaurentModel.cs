namespace MenuManagement.AI.Domain.Models;

public record RestaurentModel
{
    public string RestaurentId { get; set; } = string.Empty;
    public string RestaurantName { get; set; } = string.Empty;
    public string RestaurantDescription { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public int Rating { get; set; }
}
