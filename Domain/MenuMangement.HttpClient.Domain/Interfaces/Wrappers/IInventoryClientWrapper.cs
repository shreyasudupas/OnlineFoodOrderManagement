namespace MenuMangement.HttpClient.Domain.Interfaces.Wrappers
{
    public interface IInventoryClientWrapper
    {
        Task<string> PostApiCall<TData>(string url, string token, TData payload);
    }
}
