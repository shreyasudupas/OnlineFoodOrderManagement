using MenuManagement.MessagingQueue.Core.Models;

namespace MenuManagement.HttpClient.Domain.Interface
{
    public interface INotificationClientWrapper
    {
        Task<NotificationModel> PostApiCall<TData>(string url, string token, TData payload);
    }
}
