using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IIdsHttpClientWrapper
    {
        Task<string> AddApiCallAsync<TData>(string url, TData payload, string token, string clientName);
        Task<string> GetApiAccessToken();
    }
}
