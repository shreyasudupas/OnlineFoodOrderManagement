using MenuMangement.HttpClient.Domain.Models;

namespace MenuMangement.HttpClient.Domain.Interfaces.Wrappers;

public interface ISignalRNotificationClientWrapper
{
    Task GetCallAsync(NotificationSignalRRequest notificationSignalRRequest, string token);
}
