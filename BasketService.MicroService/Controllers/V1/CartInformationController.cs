using BasketService.MicroService.BuisnessLayer.IBuisnessLayer;
using BasketService.MicroService.Models;
using Identity.MicroService.Models.APIResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BasketService.MicroService.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
    }
}
