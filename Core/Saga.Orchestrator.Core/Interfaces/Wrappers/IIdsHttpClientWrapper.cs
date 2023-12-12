namespace Saga.Orchestrator.Core.Interfaces.Wrappers
{
    public interface IIdsHttpClientWrapper
    {
        Task<string> PostApiCallAsync<TData>(string url, TData payload, string token, string clientName);
        Task<string> GetApiAccessToken();
    }
}
