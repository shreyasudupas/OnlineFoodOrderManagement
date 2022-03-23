using Common.Utility.BuisnessLayer.IBuisnessLayer;
using Common.Utility.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Identity.MicroService.Security.Middleware
{
    public class GetUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public GetUserMiddleware(RequestDelegate next, ILogger<GetUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IProfileUser profile)
        {
            if (context.Request.Headers["UserInfo"].Count > 0)
            {
                //get UserInfo from header
                var User = JsonConvert.DeserializeObject<UserProfile>(context.Request.Headers["UserInfo"]);

                _logger.LogInformation("Identity Header Middleware called, Username: {0}",User.Username);
                if (!string.IsNullOrEmpty(User.Username))
                    profile.SetUserDetails(User.Username, User.PictureLocation, User.NickName, User.RoleId);
            }

            await _next(context);
        }
    }
}
