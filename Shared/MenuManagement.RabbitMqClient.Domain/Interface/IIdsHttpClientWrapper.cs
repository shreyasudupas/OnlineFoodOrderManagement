namespace MenuManagement.HttpClient.Domain.Interface
{
    public interface IIdsHttpClientWrapper
    {
        Task<string> AddApiCallAsync<TData>(string url, TData payload, string token, string clientName);
        Task<string> GetApiAccessToken();
    }
}
