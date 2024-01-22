namespace MenuMangement.HttpClient.Domain.Models
{
    public record NotificationSignalRRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public int NotificationCount { get; set; }
    }
}
