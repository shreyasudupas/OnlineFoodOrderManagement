using BasketService.MicroService.BuisnessLayer.IBuisnessLayer;
using MicroService.Shared.Models;
using MicroService.Shared.Models.CartInformationModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.MicroService.BuisnessLayer
{
    public class CartService : ICartService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public CartService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        //public async Task<int> AddItemsinCart(string Username,CartItems cartItems)
        //{
        //    int Items = 0;
        //    var db = _connectionMultiplexer.GetDatabase();
        //    var ifItemPresent = await db.StringGetAsync(Username);
        //    var deserilizeUserInfo = JsonConvert.DeserializeObject<UserCartInfo>(ifItemPresent);
        //    //If items added for first time
        //    if(deserilizeUserInfo.Items == null)
        //    {
        //        List<CartItems> items = new List<CartItems>();
        //        items.Add(cartItems);
        //        deserilizeUserInfo.Items = items;

        //        await updateUserCartCache(db, Username, deserilizeUserInfo);
                
        //        Items += 1;
        //    }
        //    else
        //    {
        //        //if additional item added
                
        //        var getItem = deserilizeUserInfo.Items.Find(x => x.id == cartItems.id);
        //        //if the added item is same
        //        if (getItem!=null)
        //        {
        //            getItem.quantity = cartItems.quantity;
        //        }
        //        else
        //        {
        //            //add in the cart 
        //            deserilizeUserInfo.Items.Add(cartItems);
        //        }
        //        await updateUserCartCache(db, Username, deserilizeUserInfo);

        //        //get count of total quantity
        //        return deserilizeUserInfo.Items.Sum(x=>x.quantity);
        //    }
        //    return Items;
        //}

        public async Task<bool> AddItemsInCartV2(string Username,JObject CartInfoFromHeader)
        {
            var IsSucess = false;
            string cartMenuId;
            Dictionary<string, object> menuObject;
            //This will create a object of item got from header
            CreateCartMenuObject(CartInfoFromHeader, out cartMenuId, out menuObject);

            var db = _connectionMultiplexer.GetDatabase();
            var IsUserInfo = await db.StringGetAsync(Username);
            var UserInfoInCache = JsonConvert.DeserializeObject<UserCartInformation>(IsUserInfo);

            //if item is added in cache for the first time
            if (UserInfoInCache.Items == null)
            {
                //convert List to JObjects
                JObject newObj = JObject.Parse(JsonConvert.SerializeObject(menuObject));
                UserInfoInCache.Items = new List<JObject>();
                UserInfoInCache.Items.Add(newObj);

                //update the item in cache
                await updateUserCartCacheV2(db, Username, UserInfoInCache);
                IsSucess = true;
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
                    await updateUserCartCacheV2(db, Username, UserInfoInCache);
                    IsSucess = true;
                }
                else
                {
                    UpdateUserCacheCartQuantity(menuObject, UserInfoInCache, ItemExistsInCache);

                    //update the item in cache
                    await updateUserCartCacheV2(db, Username, UserInfoInCache);
                    IsSucess = true;
                }
            }

            return IsSucess;
        }

        private static void CreateCartMenuObject(JObject CartInfoFromHeader, out string cartMenuId, out Dictionary<string, object> menuObject)
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
            menuObject[cartObject.Keys.ElementAt(ColumnsList.Count()).ToString()] = cartObject.Values.ElementAt(ColumnsList.Count());
        }

        public async Task<bool> RemoveItemsFromCartV2(string Username, JObject CartInfoFromHeader)
        {
            var IsSucess = false;
            string cartMenuId;
            Dictionary<string, object> menuObject;

            //This will create a object of item got from header
            CreateCartMenuObject(CartInfoFromHeader, out cartMenuId, out menuObject);

            var db = _connectionMultiplexer.GetDatabase();
            var IsUserInfo = await db.StringGetAsync(Username);
            var UserInfoInCache = JsonConvert.DeserializeObject<UserCartInformation>(IsUserInfo);

            //find if menu id is present in cache
            var ItemExistsInCache = (from cache in UserInfoInCache.Items
                                     where cartMenuId == (string)cache["id"]
                                     select cache).FirstOrDefault();

            if(ItemExistsInCache != null)
            {
                //get the quantity of menu Object
                var quantity = (long?)(menuObject.FirstOrDefault(x=>x.Key == "quantity")).Value;

                if(quantity.Value == 0)
                {
                    //remove the item from the list
                    UserInfoInCache.Items.Remove(ItemExistsInCache);

                    if(UserInfoInCache.Items.Count > 0)
                    {
                        //update the item in cache
                        await updateUserCartCacheV2(db, Username, UserInfoInCache);
                        IsSucess = true;
                    }
                    else
                    {
                        UserInfoInCache.Items = null;
                        //update the item in cache
                        await updateUserCartCacheV2(db, Username, UserInfoInCache);
                        IsSucess = true;
                    }
                }
                else//Then reduce the item count and update in cache
                {
                    UpdateUserCacheCartQuantity(menuObject, UserInfoInCache, ItemExistsInCache);

                    //update the item in cache
                    await updateUserCartCacheV2(db, Username, UserInfoInCache);
                    IsSucess = true;
                }
            }

            return IsSucess;
        }

        private static void UpdateUserCacheCartQuantity(Dictionary<string, object> menuObject, UserCartInformation UserInfoInCache, JObject ItemExistsInCache)
        {
            UserInfoInCache.Items.Remove(ItemExistsInCache);

            JObject newCartItem = JObject.Parse(JsonConvert.SerializeObject(menuObject));
            //add item in Item back with updated value
            UserInfoInCache.Items.Add(newCartItem);
        }

        public async Task<UserCartInfo> GetItemsFromCacheCart(string Username)
        {
            //List<CartItems> items = new List<CartItems>();
            UserCartInfo userCartInfo = new UserCartInfo();  
            var db = _connectionMultiplexer.GetDatabase();
            var getUserInfo = await db.StringGetAsync(Username);
            if(getUserInfo.HasValue)
            {
                userCartInfo = JsonConvert.DeserializeObject<UserCartInfo>(getUserInfo);
                
            }
            return userCartInfo;
        }

        //public async Task<int> RemoveItemsFromCart(string Username, CartItems cartItems)
        //{
        //    int Items = 0;
        //    var db = _connectionMultiplexer.GetDatabase();
        //    var isItemsPresent = await db.StringGetAsync(Username);
        //    if(isItemsPresent.HasValue)
        //    {
        //        var GetUserInfo = JsonConvert.DeserializeObject<UserCartInfo>(isItemsPresent);
        //        //List<CartItems> items = JsonConvert.DeserializeObject<List<CartItems>>(isItemsPresent);

        //        Items = await UpdateCartRecord(cartItems, GetUserInfo, Username, db);
        //    }

        //    return Items;
        //}

        public async Task<int> UpdateCartRecord(CartItems CartItemFromUI, UserCartInfo CartItemFromCache, string Username,IDatabase db)
        {
            var ItemsCount = 0;
            var getItem = CartItemFromCache.Items.Find(x => x.id == CartItemFromUI.id);

            if(getItem!= null)
            {
                if (CartItemFromUI.quantity == 0)
                {
                    //remove the current value from cache
                    CartItemFromCache.Items.Remove(getItem);

                    if (CartItemFromCache.Items.Count > 0)
                        //await updateCartCache(db, Username, CartItemFromCache);
                        await updateUserCartCache(db, Username, CartItemFromCache);
                    else
                    //await db.KeyDeleteAsync(Username);
                    {
                        CartItemFromCache.Items = null;
                        await updateUserCartCache(db, Username, CartItemFromCache);
                    }
                }
                else
                {
                    //more than 1 quantity is present then reduce the quantity and updat the cache
                    //var getItem = CartItemFromCache.Find(x => x.id == CartItemFromUI.id);
                    getItem.quantity -= 1;
                    List<CartItems> newItem = new List<CartItems>();
                    newItem.Add(getItem);
                    var otherItems = CartItemFromCache.Items.FindAll(x => x.id != CartItemFromUI.id);
                    newItem.AddRange(otherItems);

                    CartItemFromCache.Items = newItem;
                    //await updateCartCache(db, Username, newItem);
                    await updateUserCartCache(db, Username, CartItemFromCache);
                    ItemsCount = newItem.Sum(x => x.quantity);
                }
            }
            
            return ItemsCount;
        }

        public async Task updateUserCartCacheV2(IDatabase db, string Username, UserCartInformation items)
        {
            //remove the current value from cache
            await db.KeyDeleteAsync(Username);
            //add the updated value
            var serilizeItem = JsonConvert.SerializeObject(items);
            await db.StringSetAsync(Username, serilizeItem);
        }

        public async Task updateUserCartCache(IDatabase db, string Username, UserCartInfo items)
        {
            //remove the current value from cache
            await db.KeyDeleteAsync(Username);
            //add the updated value
            var serilizeItem = JsonConvert.SerializeObject(items);
            await db.StringSetAsync(Username, serilizeItem);
        }

        public async Task<string> GetAllBasketUserMenuList(string Username)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var IsUserInfo = await db.StringGetAsync(Username);
            var UserInfoInCache = JsonConvert.DeserializeObject<UserCartInformation>(IsUserInfo);
            var Items = JsonConvert.SerializeObject( new MenuCartResponse { 
             UserInfo = UserInfoInCache.UserInfo,
             Items = UserInfoInCache.Items
            });
            


            return Items;
        }
    }
}
