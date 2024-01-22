using MenuMangement.HttpClient.Domain.Model;

namespace MenuMangement.HttpClient.Domain.Interfaces.Wrappers
{
    public interface INotificationClientWrapper
    {
        Task<NotificationModel> PostApiCall<TData>(string url, string token, TData payload);
    }
}
