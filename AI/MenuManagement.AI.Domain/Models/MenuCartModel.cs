namespace MenuManagement.AI.Domain.Models;

public record MenuCartModel
{
    public string MenuId { get; set; } = string.Empty;

    public string MenuItemName { get; set; } = string.Empty;

    public int Quantity { get; set; }
}
