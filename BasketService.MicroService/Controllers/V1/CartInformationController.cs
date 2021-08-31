using BasketService.MicroService.BuisnessLayer.IBuisnessLayer;
using BasketService.MicroService.Models;
using Identity.MicroService.Models.APIResponse;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BasketService.MicroService.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class CartInformationController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartInformationController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<Response> AddItemInCache()
        {
            var header = HttpContext.Request.Headers["cart-info"];
            var Userheader = HttpContext.Request.Headers["UserInfo"];
            var UserInfo = JsonConvert.DeserializeObject<UserHeader>(Userheader);
            var result = false;
            //var data = JsonConvert.SerializeObject(Item);
            if (header.Count >0)
            {
                var Obj = JObject.Parse(header);
                result = await cartService.AddItemsInCartV2(UserInfo.Username, Obj);
            }
            
            return new Response(HttpStatusCode.OK, result, null);
        }

        [HttpGet]
        public async Task<Response> RemoveItemsFromCache()
        {
            var header = HttpContext.Request.Headers["cart-info"];
            var Userheader = HttpContext.Request.Headers["UserInfo"];
            var UserInfo = JsonConvert.DeserializeObject<UserHeader>(Userheader);

            var result = false;
            //var data = JsonConvert.SerializeObject(Item);
            if (header.Count>0)
            {
                var Obj = JObject.Parse(header);
                result = await cartService.RemoveItemsFromCartV2(UserInfo.Username, Obj);
            }
            return new Response(HttpStatusCode.OK, result, null);
        }

        [HttpGet]
        public async Task<Response> GetUserBasketInfoFromCache()
        {
            var Userheader = HttpContext.Request.Headers["UserInfo"];
            if(Userheader.Count > 0)
            {
                var UserInfo = JsonConvert.DeserializeObject<UserHeader>(Userheader);
                var result = await cartService.GetAllBasketUserMenuList(UserInfo.Username);
                return new Response(HttpStatusCode.OK, result, null);
            }
            else
            {
                return new Response(HttpStatusCode.InternalServerError, null, "No user Header value was present");
            }
        }

        public class CartItems
        {
            public List<MenuColumnData> ColumnData { get; set; }
            
            public object Data { get; set; }
        }

        public class MenuColumnData
        {
            public string Field { get; set; }
            public string Header { get; set; }
            public string Display { get; set; }
        }

        public class UserCartInfoDemo
        {
            public UserInfo UserInfo { get; set; }
            public List<JObject> Items { get; set; }
        }

        public class UserCartInfoDemoOutPut
        {
            public UserInfo UserInfo { get; set; }
            public List<Dictionary<string,object>> Items { get; set; }
        }

        public class MenuItem
        {
            public string MenuId { get; set; }
            public int? Quantity { get; set; }
        }

    }
}
