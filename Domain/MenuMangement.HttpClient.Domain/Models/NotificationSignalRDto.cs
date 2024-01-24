namespace MenuMangement.HttpClient.Domain.Models
{
    public record NotificationSignalRRequest
    {
        public string FromUserId { get; set; }
        public bool isSendAll { get; set; }

        public string ToUserId { get; set; }
        public int NotificationCount { get; set; }
    }
}
