namespace MenuManagement.SignalR.HubService.Common.Models;

public class HubUser
{
    public string UserId { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

