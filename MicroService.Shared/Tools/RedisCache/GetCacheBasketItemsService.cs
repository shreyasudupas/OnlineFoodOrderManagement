using Common.Utility.Models.CartInformationModels;
using Common.Utility.Tools.RedisCache.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Common.Utility.Tools.RedisCache
{
    public class GetCacheBasketItemsService: IGetCacheBasketItemsService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private readonly ILogger _logger;

        public GetCacheBasketItemsService(IConnectionMultiplexer connectionMultiplexer, ILogger<GetCacheBasketItemsService> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _logger = logger;
        }

        public async Task<UserCartInformation> GetBasketItems(string Username)
        {
            _logger.LogInformation("User: {0} GetBasketItems started",Username);

            var IsUserInfo = await _database.StringGetAsync(Username);
            if (IsUserInfo.HasValue)
            {
                _logger.LogInformation("User: {0} GetBasketItems success", Username);

                return JsonConvert.DeserializeObject<UserCartInformation>(IsUserInfo);
            }
            else
            {
                _logger.LogInformation("User: {0} GetBasketItems no item with this username", Username);
                return new UserCartInformation();
            }
        }

        public async Task<bool> UpdateBasketItems(string Username,UserCartInformation userCartInformation)
        {
            bool IsSuccess;
            _logger.LogInformation("User: {0} UpdateBasketItems started", Username);

            //remove the current value from cache
            await _database.KeyDeleteAsync(Username);
            //add the updated value
            var serilizeItem = JsonConvert.SerializeObject(userCartInformation);
            IsSuccess = await _database.StringSetAsync(Username, serilizeItem);

            _logger.LogInformation("User: {0} UpdateBasketItems ended with success {1}", Username,IsSuccess);
            return IsSuccess;
        }

        public async Task<RedisValue> GetCacheItem(string Username)
        {
            _logger.LogInformation("User: {0} GetCacheItem started", Username);
            var getUserInfoFromCache = await _database.StringGetAsync(Username);
            _logger.LogInformation("User: {0} GetCacheItem has value {1} -- function end", Username,getUserInfoFromCache.HasValue);

            return getUserInfoFromCache;
            
        }

        public async Task<bool> SetCacheItem(string Username,object Information)
        {
            _logger.LogInformation("User: {0} SetCacheItem started", Username);
            var IsSucess = await _database.StringSetAsync(Username, JsonConvert.SerializeObject(Information));
            _logger.LogInformation("User: {0} SetCacheItem end with success {1}", Username, IsSucess);

            return IsSucess;

        }
    }
}
