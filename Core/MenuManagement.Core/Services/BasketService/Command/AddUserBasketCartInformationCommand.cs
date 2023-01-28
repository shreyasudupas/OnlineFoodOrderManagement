using MediatR;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.BasketService.Command
{
    public class AddUserBasketCartInformationCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public JObject CartInformation { get; set; }
    }

    public class AddUserBasketCartInformationCommandHandler : IRequestHandler<AddUserBasketCartInformationCommand, bool>
    {
        private readonly IRedisCacheBasketService _redisCacheBasketService;
        private readonly ILogger<AddUserBasketCartInformationCommandHandler> _logger;

        public AddUserBasketCartInformationCommandHandler(IRedisCacheBasketService redisCacheBasketService,
            ILogger<AddUserBasketCartInformationCommandHandler> logger)
        {
            _redisCacheBasketService = redisCacheBasketService;
            _logger = logger;
        }

        public async Task<bool> Handle(AddUserBasketCartInformationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ManageUserBasketCartInformationCommand for username: {0} started", request.Username);

            var isSucess = false;
            string cartMenuId;
            Dictionary<string, object> menuObject;
            VendorDetail vendorDetail = new VendorDetail();

            //This will create a object of item got from header
            BasketCommon.CreateCartMenuObject(request.CartInformation, out cartMenuId, out menuObject, out vendorDetail);


            var userBasket = await _redisCacheBasketService.GetBasketItems(request.Username);

            //if item is added in cache for the first time
            if (userBasket.Items == null)
            {
                //convert List to JObjects
                JObject newObj = JObject.Parse(JsonConvert.SerializeObject(menuObject));
                userBasket.Items = new List<JObject>();
                userBasket.Items.Add(newObj);
                userBasket.VendorDetails = vendorDetail;


                isSucess = await _redisCacheBasketService.UpdateBasketItems(request.Username, userBasket);
            }
            else
            {
                //find if menu id is present in cache
                var ItemExistsInCache = (from cache in userBasket.Items
                                         where cartMenuId == (string)cache["id"]
                                         select cache).FirstOrDefault();

                if (ItemExistsInCache == null)
                {
                    //convert List to JObjects
                    JObject ConvertAddedItemInCache = JObject.Parse(JsonConvert.SerializeObject(menuObject));
                    userBasket.Items.Add(ConvertAddedItemInCache);

                    //update the item in cache
                    isSucess = await _redisCacheBasketService.UpdateBasketItems(request.Username, userBasket);
                }
                else
                {
                    BasketCommon.UpdateUserCacheCartQuantity(menuObject, userBasket, ItemExistsInCache);

                    //update the item in cache
                    isSucess = await _redisCacheBasketService.UpdateBasketItems(request.Username, userBasket);
                }
            }

            _logger.LogInformation("ManageUserBasketCartInformationCommand for username: {0} ended with success {1}", request.Username, isSucess);
            return isSucess;
        }
    }
}
