namespace MenuManagement.HttpClient.Domain.Interface
{
    public interface IInventoryClientWrapper
    {
        Task<string> PostApiCall<TData>(string url, string token, TData payload);
    }
}
