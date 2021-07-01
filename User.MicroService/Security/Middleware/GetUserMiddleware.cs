using MicroService.Shared.BuisnessLayer.IBuisnessLayer;
using MicroService.Shared.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Identity.MicroService.Security.Middleware
{
    public class GetUserMiddleware
    {
        private readonly RequestDelegate _next;
        public GetUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IProfileUser profile)
        {
            if (context.Request.Headers["UserInfo"].Count > 0)
            {
                //get UserInfo from header
                var User = JsonConvert.DeserializeObject<UserProfile>(context.Request.Headers["UserInfo"]);

                if (!string.IsNullOrEmpty(User.Username))
                    profile.SetUserDetails(User.Username, User.PictureLocation, User.NickName, User.RoleId);
            }

            await _next(context);
        }
    }
}
