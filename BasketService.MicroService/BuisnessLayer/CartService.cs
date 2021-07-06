using BasketService.MicroService.BuisnessLayer.IBuisnessLayer;
using MicroService.Shared.Models;
using Newtonsoft.Json;
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
        public async Task<int> AddItemsinCart(string Username,CartItems cartItems)
        {
            int Items = 0;
            var db = _connectionMultiplexer.GetDatabase();
            var ifItemPresent = await db.StringGetAsync(Username);
            var deserilizeUserInfo = JsonConvert.DeserializeObject<UserCartInfo>(ifItemPresent);
            //If items added for first time
            if(deserilizeUserInfo.Items == null)
            {
                List<CartItems> items = new List<CartItems>();
                items.Add(cartItems);
                deserilizeUserInfo.Items = items;

                await updateUserCartCache(db, Username, deserilizeUserInfo);
                //var itemsToBeStored = JsonConvert.SerializeObject(items);
                //await db.StringSetAsync(Username, itemsToBeStored);
                Items += 1;
            }
            else
            {
                //if additional item added
                //var ItemConvert = JsonConvert.DeserializeObject<List<CartItems>>(ifItemPresent);
                //var getItem = ItemConvert.Find(x =>x.id == cartItems.id);
                var getItem = deserilizeUserInfo.Items.Find(x => x.id == cartItems.id);
                //if the added item is same
                if (getItem!=null)
                {
                    getItem.quantity = cartItems.quantity;
                }
                else
                {
                    //cartItems.quantity += 1;
                    //add in the cart 
                    deserilizeUserInfo.Items.Add(cartItems);
                }
                await updateUserCartCache(db, Username, deserilizeUserInfo);

                //get count of total quantity
                return deserilizeUserInfo.Items.Sum(x=>x.quantity);
            }
            return Items;
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

        public async Task<int> RemoveItemsFromCart(string Username, CartItems cartItems)
        {
            int Items = 0;
            var db = _connectionMultiplexer.GetDatabase();
            var isItemsPresent = await db.StringGetAsync(Username);
            if(isItemsPresent.HasValue)
            {
                var GetUserInfo = JsonConvert.DeserializeObject<UserCartInfo>(isItemsPresent);
                //List<CartItems> items = JsonConvert.DeserializeObject<List<CartItems>>(isItemsPresent);

                Items = await UpdateCartRecord(cartItems, GetUserInfo, Username, db);
            }

            return Items;
        }

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

        public async Task updateCartCache(IDatabase db,string Username,List<CartItems> items)
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
    }
}
