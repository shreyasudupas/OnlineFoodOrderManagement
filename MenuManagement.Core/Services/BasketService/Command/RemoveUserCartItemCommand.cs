using MediatR;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.BasketService.Command
{
    public class RemoveUserCartItemCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public JObject CartInformation { get; set; }
    }

    public class RemoveUserCartItemCommandHandler : IRequestHandler<RemoveUserCartItemCommand, bool>
    {
        private readonly ILogger<RemoveUserCartItemCommandHandler> _logger;
        private readonly IRedisCacheBasketService _redisCacheBasketService;

        public RemoveUserCartItemCommandHandler(ILogger<RemoveUserCartItemCommandHandler> logger
            , IRedisCacheBasketService redisCacheBasketService)
        {
            _logger = logger;
            _redisCacheBasketService = redisCacheBasketService;
        }
        public async Task<bool> Handle(RemoveUserCartItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("RemoveUserCartItemCommandHandler for username: {0} started", request.Username);

            var IsSucess = false;
            string cartMenuId;
            Dictionary<string, object> menuObject;
            VendorDetail vendorDetail = new VendorDetail();

            //This will create a object of item got from header
            BasketCommon.CreateCartMenuObject(request.CartInformation, out cartMenuId, out menuObject, out vendorDetail);

            var UserInfoInCache = await _redisCacheBasketService.GetBasketItems(request.Username);

            if (UserInfoInCache.Items != null)
            {
                //find if menu id is present in cache
                var ItemExistsInCache = (from cache in UserInfoInCache.Items
                                         where cartMenuId == (string)cache["id"]
                                         select cache).FirstOrDefault();

                if (ItemExistsInCache != null)
                {
                    //get the quantity of menu Object
                    var quantity = (long?)(menuObject.FirstOrDefault(x => x.Key == "quantity")).Value;

                    if (quantity.Value == 0)
                    {
                        //remove the item from the list
                        UserInfoInCache.Items.Remove(ItemExistsInCache);

                        if (UserInfoInCache.Items.Count > 0)
                        {
                            //update the item in cache
                            IsSucess = await _redisCacheBasketService.UpdateBasketItems(request.Username, UserInfoInCache);
                        }
                        else
                        {
                            UserInfoInCache.Items = null;
                            UserInfoInCache.VendorDetails = null;
                            //update the item in cache
                            IsSucess = await _redisCacheBasketService.UpdateBasketItems(request.Username, UserInfoInCache);
                        }
                    }
                    else//Then reduce the item count and update in cache
                    {
                        BasketCommon.UpdateUserCacheCartQuantity(menuObject, UserInfoInCache, ItemExistsInCache);

                        //update the item in cache
                        IsSucess = await _redisCacheBasketService.UpdateBasketItems(request.Username, UserInfoInCache);
                    }
                }
            }
            _logger.LogInformation("RemoveUserCartItemCommandHandler for username: {0} eneded with success: {1}", request.Username, IsSucess);

            return IsSucess;
        }
    }
}
