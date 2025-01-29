namespace MenuManagement.AI.Domain.Models;

public record UserCartModel
{
    public string UserId { get; set; } = Guid.NewGuid().ToString();

    //menu Id and quantity
    public Dictionary<string, MenuCartModel> CartInformations { get; set; } = new();
}
