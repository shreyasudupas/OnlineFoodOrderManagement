using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.BasketService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace MenOrder.Infrastructure.Services
{
    public class RedisCacheBasketService : IRedisCacheBasketService
    {
        private readonly ILogger<RedisCacheBasketService> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;

        public RedisCacheBasketService(ILogger<RedisCacheBasketService> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<UserCartInformation> GetBasketItems(string Username)
        {
            _logger.LogInformation("User: {0} GetBasketItems started", Username);

            var userInfo = await _database.StringGetAsync(Username);
            if (userInfo.HasValue)
            {
                _logger.LogInformation("User: {0} GetBasketItems success", Username);

                return JsonConvert.DeserializeObject<UserCartInformation>(userInfo);
            }
            else
            {
                _logger.LogInformation("User: {0} GetBasketItems no item with this username", Username);
                return new UserCartInformation();
            }
        }

        public async Task<bool> UpdateBasketItems(string Username, UserCartInformation userCartInformation)
        {
            bool IsSuccess;
            _logger.LogInformation("User: {0} UpdateBasketItems started", Username);

            //remove the current value from cache
            await _database.KeyDeleteAsync(Username);

            //add the updated value
            var serilizeItem = JsonConvert.SerializeObject(userCartInformation);
            IsSuccess = await _database.StringSetAsync(Username, serilizeItem);

            _logger.LogInformation("User: {0} UpdateBasketItems ended with success {1}", Username, IsSuccess);
            return IsSuccess;
        }
    }
}
