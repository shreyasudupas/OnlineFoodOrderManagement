using MenuMangement.HttpClient.Domain.Models;

namespace MenuMangement.HttpClient.Domain.Interfaces.Wrappers;

public interface ISignalRNotificationClient
{
    Task GetAsyncCall(NotificationSignalRRequest notificationSignalRRequest, string token);
}
