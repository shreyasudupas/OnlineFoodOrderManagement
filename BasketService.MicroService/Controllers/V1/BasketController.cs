using BasketService.MicroService.BuisnessLayer.IBuisnessLayer;
using BasketService.MicroService.Models;
using Identity.MicroService.Models.APIResponse;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.MicroService.Controllers.V1
{
    [Route("api/V1/[controller]/[action]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ICartService _cartS;

        public BasketController(IConnectionMultiplexer connectionMultiplexer, ICartService cartS)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _cartS = cartS;
        }

        [HttpGet]
        public async Task<APIResponse> GetUserValue()
        {
            APIResponse response = new APIResponse();

            var header = HttpContext.Request.Headers["UserInfo"];
            var UserInfo = JsonConvert.DeserializeObject<UserHeader>(header);

            var db = _connectionMultiplexer.GetDatabase();
             var res = await db.StringGetAsync(UserInfo.Username);
            response.Content = JsonConvert.DeserializeObject<UserHeader>(res);
            response.Response = 200;
            return response;
        }
        [HttpPost]
        public async Task<APIResponse> StoreUserKeyValue(UserHeader user)
        {
            APIResponse response = new APIResponse();

            var db = _connectionMultiplexer.GetDatabase();
            var UserSerilization = JsonConvert.SerializeObject(new { username = user.Username , nickname = user.Nickname });
            var options = new TimeSpan(1, 0, 0);

            var isCachePresent = await db.StringGetAsync(user.Username);
            //check if the key is present
            if(!isCachePresent.HasValue)
            {
                await db.StringSetAsync(user.Username, UserSerilization, options);
                response.Content = "Stored successfully";
            }
            else
            {
                response.Content = "Item already present";
            }

            
            response.Response = 200;
            return response;
        }

        [HttpDelete]
        public async Task<APIResponse> DeleteKey(string key)
        {
            APIResponse response = new APIResponse();
            var db = _connectionMultiplexer.GetDatabase();
            var res = await db.KeyDeleteAsync(key);
            response.Content = res?"Deleted successfully":"Error in Deleting";
            response.Response = 200;
            return response;
        }


        //[HttpPost]
        //public async Task<Response> StoreUserBasketValue(CartItems cartItems)
        //{
        //    var User = JsonConvert.DeserializeObject<UserHeader>(HttpContext.Request.Headers["UserInfo"]);
        //    var result = await _cartS.AddItemsinCart(User.Username, cartItems);
        //    if(result>0 )
        //    {
        //        return new Response(System.Net.HttpStatusCode.OK, result, null);
        //    }
        //    else
        //    {
        //        return new Response(System.Net.HttpStatusCode.NotFound, null, null);
        //    }
        //}
        //[HttpGet]
        //public async Task<Response> GetUserBasket()
        //{
        //    var User = JsonConvert.DeserializeObject<UserHeader>(HttpContext.Request.Headers["UserInfo"]);
        //    var result =await _cartS.GetItemsFromCacheCart(User.Username);
        //    if (result != null)
        //    {
        //        return new Response(System.Net.HttpStatusCode.OK, result, null);
        //    }
        //    else
        //        return new Response(System.Net.HttpStatusCode.NotFound, null, null);
        //}

        //[HttpPost]
        //public async Task<Response> UpdateUserBasket(CartItems cartItems)
        //{
        //    var User = JsonConvert.DeserializeObject<UserHeader>(HttpContext.Request.Headers["UserInfo"]);
        //    var result = await _cartS.RemoveItemsFromCart(User.Username,cartItems);
        //    if (result >= 0)
        //    {
        //        return new Response(System.Net.HttpStatusCode.OK, result, null);
        //    }
        //    else
        //        return new Response(System.Net.HttpStatusCode.NotFound, null, null);
        //}
    }
}
