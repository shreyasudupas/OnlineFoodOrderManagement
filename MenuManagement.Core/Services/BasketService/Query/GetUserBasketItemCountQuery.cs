using MediatR;
using MenuManagement.Core.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.BasketService.Query
{
    public class GetUserBasketItemCountQuery : IRequest<int>
    {
        public string Username { get; set; }
    }

    public class GetUserBasketItemCountQueryHandler : IRequestHandler<GetUserBasketItemCountQuery, int>
    {
        private readonly ILogger<GetUserBasketItemCountQueryHandler> _logger;
        private readonly IRedisCacheBasketService _redisCacheBasketService;

        public GetUserBasketItemCountQueryHandler(ILogger<GetUserBasketItemCountQueryHandler> logger,IRedisCacheBasketService redisCacheBasketService)
        {
            _logger = logger;
            _redisCacheBasketService = redisCacheBasketService;
        }

        public async Task<int> Handle(GetUserBasketItemCountQuery request, CancellationToken cancellationToken)
        {
            var userInfoInCache = await _redisCacheBasketService.GetBasketItems(request.Username);

            _logger.LogInformation($"User Basket Info {JsonConvert.SerializeObject(userInfoInCache)}");

            if (userInfoInCache.Items != null)
            {
                return userInfoInCache.Items.Select(x => Convert.ToInt32(x["quantity"])).Sum(x => x);
            }
            else
                return 0;
        }
    }
}
