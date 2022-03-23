using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MenuManagement.Core.Common.Models.BasketService;
using MenuManagement.Core.Common.Interfaces;

namespace MenuManagement.Core.Services.BasketService.Query
{
    public class GetBasketInformationQuery : IRequest<string>
    {
        public string Username { get; set; }
    }

    public class GetBasketInformationQueryHandler : IRequestHandler<GetBasketInformationQuery, string>
    {
        private readonly ILogger<GetBasketInformationQueryHandler> _logger;
        private readonly IRedisCacheBasketService _redisCacheBasketService;

        public GetBasketInformationQueryHandler(ILogger<GetBasketInformationQueryHandler> logger, IRedisCacheBasketService redisCacheBasketService)
        {
            _logger = logger;
            _redisCacheBasketService = redisCacheBasketService;
        }

        public async Task<string> Handle(GetBasketInformationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllBasketUserMenuList called for user {0}", request.Username);

            var userInfoInCache = await _redisCacheBasketService.GetBasketItems(request.Username);
            var items = JsonConvert.SerializeObject(new MenuCartResponse
            {
                UserInfo = userInfoInCache.UserInfo,
                Items = userInfoInCache.Items,
                VendorDetails = userInfoInCache.VendorDetails
            });

            _logger.LogInformation("GetAllBasketUserMenuList ended for user {0} with item success", request.Username);

            return items;
        }
    }
}
