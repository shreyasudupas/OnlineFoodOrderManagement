using BasketService.MicroService.BuisnessLayer.IBuisnessLayer;
using Common.Utility.Models.CartInformationModels;
using Common.Utility.Tools.RedisCache.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.MicroService.BuisnessLayer
{
    public class CartService : ICartService
    {
        private readonly IGetCacheBasketItemsService _getCacheBasketService;
        private readonly ILogger _logger;

        public CartService(IGetCacheBasketItemsService getCacheBasketItemsService,ILogger<CartService> logger)
        {
            _getCacheBasketService = getCacheBasketItemsService;
            _logger = logger;
        }

        public async Task<bool> AddItemsInCartV2(string Username,JObject CartInfoFromHeader)
        {
            _logger.LogInformation("AddItemsInCartV2 for username: {0} started",Username);

            var IsSucess = false;
            string cartMenuId;
            Dictionary<string, object> menuObject;
            VendorDetail vendorDetail=new VendorDetail();
            //This will create a object of item got from header
            CreateCartMenuObject(CartInfoFromHeader, out cartMenuId, out menuObject,out vendorDetail);

            
            var UserInfoInCache = await _getCacheBasketService.GetBasketItems(Username);

            //if item is added in cache for the first time
            if (UserInfoInCache.Items == null)
            {
                //convert List to JObjects
                JObject newObj = JObject.Parse(JsonConvert.SerializeObject(menuObject));
                UserInfoInCache.Items = new List<JObject>();
                UserInfoInCache.Items.Add(newObj);
                UserInfoInCache.VendorDetails = vendorDetail;

                
                IsSucess = await _getCacheBasketService.UpdateBasketItems(Username, UserInfoInCache);
            }
            else
            {
                //find if menu id is present in cache
                var ItemExistsInCache = (from cache in UserInfoInCache.Items
                                         where cartMenuId == (string)cache["id"]
                                         select cache).FirstOrDefault();

                if (ItemExistsInCache == null)
                {
                    //convert List to JObjects
                    JObject ConvertAddedItemInCache = JObject.Parse(JsonConvert.SerializeObject(menuObject));
                    UserInfoInCache.Items.Add(ConvertAddedItemInCache);

                    //update the item in cache
                    IsSucess = await _getCacheBasketService.UpdateBasketItems(Username, UserInfoInCache);
                }
                else
                {
                    UpdateUserCacheCartQuantity(menuObject, UserInfoInCache, ItemExistsInCache);

                    //update the item in cache
                    IsSucess = await _getCacheBasketService.UpdateBasketItems(Username, UserInfoInCache);
                }
            }

            _logger.LogInformation("AddItemsInCartV2 for username: {0} ended with success {1}", Username,IsSucess);
            return IsSucess;
        }

        private static void CreateCartMenuObject(JObject CartInfoFromHeader, out string cartMenuId, out Dictionary<string, object> menuObject,out VendorDetail vendorDetail)
        {
            //Get the menu id from the selected object of the user
            cartMenuId = (string)CartInfoFromHeader["Data"]["id"];
            //convert the item selected by user to object
            var cartObject = CartInfoFromHeader["Data"].ToObject<Dictionary<string, object>>();
            menuObject = new Dictionary<string, object>();
            var ColumnsList = (from columns in CartInfoFromHeader["ColumnData"]
                               select columns).ToList();

            //prepare the object with column names and add the values of user selected item
            for (int i = 0; i < ColumnsList.Count(); i++)
            {
                menuObject[cartObject.Keys.ElementAt(i).ToString()] = cartObject.Values.ElementAt(i);
            }
            //for vendor object
            //menuObject[cartObject.Keys.ElementAt(ColumnsList.Count()).ToString()] = cartObject.Values.ElementAt(ColumnsList.Count());
            vendorDetail = CartInfoFromHeader["vendor details"].ToObject<VendorDetail>();
        }

        public async Task<bool> RemoveItemsFromCartV2(string Username, JObject CartInfoFromHeader)
        {
            _logger.LogInformation("RemoveItemsFromCartV2 for username: {0} started", Username);

            var IsSucess = false;
            string cartMenuId;
            Dictionary<string, object> menuObject;
            VendorDetail vendorDetail = new VendorDetail();
            //This will create a object of item got from header
            CreateCartMenuObject(CartInfoFromHeader, out cartMenuId, out menuObject,out vendorDetail);

            var UserInfoInCache = await _getCacheBasketService.GetBasketItems(Username);

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
                            IsSucess = await _getCacheBasketService.UpdateBasketItems(Username, UserInfoInCache);
                        }
                        else
                        {
                            UserInfoInCache.Items = null;
                            UserInfoInCache.VendorDetails = null;
                            //update the item in cache
                            IsSucess = await _getCacheBasketService.UpdateBasketItems(Username, UserInfoInCache);
                        }
                    }
                    else//Then reduce the item count and update in cache
                    {
                        UpdateUserCacheCartQuantity(menuObject, UserInfoInCache, ItemExistsInCache);

                        //update the item in cache
                        IsSucess = await _getCacheBasketService.UpdateBasketItems(Username, UserInfoInCache);
                    }
                }
            }
            _logger.LogInformation("RemoveItemsFromCartV2 for username: {0} eneded with success {1}", Username,IsSucess);

            return IsSucess;
        }

        private static void UpdateUserCacheCartQuantity(Dictionary<string, object> menuObject, UserCartInformation UserInfoInCache, JObject ItemExistsInCache)
        {
            UserInfoInCache.Items.Remove(ItemExistsInCache);

            JObject newCartItem = JObject.Parse(JsonConvert.SerializeObject(menuObject));
            //add item in Item back with updated value
            UserInfoInCache.Items.Add(newCartItem);
        }

        public async Task<string> GetAllBasketUserMenuList(string Username)
        {
            _logger.LogInformation("GetAllBasketUserMenuList called for user {0}",Username);

            var UserInfoInCache =await _getCacheBasketService.GetBasketItems(Username);
            var Items = JsonConvert.SerializeObject(new MenuCartResponse
            {
                UserInfo = UserInfoInCache.UserInfo,
                Items = UserInfoInCache.Items,
                VendorDetails = UserInfoInCache.VendorDetails
            });

            _logger.LogInformation("GetAllBasketUserMenuList ended for user {0} with item success", Username);

            return Items;
        }
    }
}
