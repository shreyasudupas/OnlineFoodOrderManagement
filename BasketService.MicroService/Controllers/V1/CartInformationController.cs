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
            
            //foreach (JProperty property in Obj.Properties())
            //{
            //    Console.WriteLine(property.Name + " - " + property.Value);
            //}
            //var cartMenuId = (string)Obj["Data"]["id"];
            //var cartObject = Obj["Data"].ToObject<Dictionary<string,object>>();
            //var menuObject = new Dictionary<string, object>();

            //var ColumnsList = (from columns in Obj["ColumnData"]
            //                     select columns).ToList();

            ////prepare the object with column names
            //for(int i = 0;i< ColumnsList.Count();i++)
            //{
            //    menuObject[cartObject.Keys.ElementAt(i).ToString()] = cartObject.Values.ElementAt(i);
            //}


            //var db = _connectionMultiplexer.GetDatabase();
            //var ifItemPresent =  db.StringGetAsync("admin@test.com").GetAwaiter().GetResult();
            //var deserilizeUserInfo = JsonConvert.DeserializeObject<UserCartInfoDemo>(ifItemPresent);

            //if(deserilizeUserInfo.Items != null)
            //{
            //    //find if menu id is present
            //    var ItemExists = (from cache in deserilizeUserInfo.Items
            //                   where cartMenuId == (string)cache["id"]
            //                   select cache).FirstOrDefault();

            //    if(ItemExists != null)
            //    {
            //        //then updat the cart info
            //         deserilizeUserInfo.Items.Remove(ItemExists);

            //        JObject newCartItem = JObject.Parse(JsonConvert.SerializeObject(menuObject));
            //        //add item in Item back with updated value
            //        deserilizeUserInfo.Items.Add(newCartItem);
            //    }

            //}
            //else
            //{
            //    List<Dictionary<string, object>> MenuItemsInCart = new List<Dictionary<string, object>>();
            //    MenuItemsInCart.Add(menuObject);
                
            //    //assign to userInfo
            //    UserCartInfoDemoOutPut output = new UserCartInfoDemoOutPut();
            //    output.UserInfo = deserilizeUserInfo.UserInfo;
            //    output.Items = MenuItemsInCart;

            //    //remove the current value from cache
            //    db.KeyDeleteAsync(output.UserInfo.UserName).GetAwaiter().GetResult();
            //    //add the updated value
            //    var serilizeItem = JsonConvert.SerializeObject(output);
            //    db.StringSetAsync(output.UserInfo.UserName, serilizeItem).GetAwaiter().GetResult();
            //}


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
