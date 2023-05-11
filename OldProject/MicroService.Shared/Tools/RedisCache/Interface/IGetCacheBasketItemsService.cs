using Common.Utility.Models.CartInformationModels;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Common.Utility.Tools.RedisCache.Interface
{
    public interface IGetCacheBasketItemsService
    {
        public Task<UserCartInformation> GetBasketItems(string Username);
        public Task<bool> UpdateBasketItems(string Username, UserCartInformation userCartInformation);
        public Task<RedisValue> GetCacheItem(string Username);
        public Task<bool> SetCacheItem(string Username, object Information);
    }
}
