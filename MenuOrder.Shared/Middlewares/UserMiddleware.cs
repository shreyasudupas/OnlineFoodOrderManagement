using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MenuOrder.Shared.Middlewares
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,IProfileUser profile)
        {
            var claims = context.User.Claims.ToList();

            if(claims.Count > 0)
            {
                if(claims.Any(u=>u.Type == "UserId"))
                {
                    var userId = claims.Find(u => u.Type == "UserId").Value;
                    profile.UserId = userId;
                }

                if(claims.Any(u=>u.Type == "Username"))
                {
                    var username = claims.Find(u => u.Type == "Username").Value;
                    profile.Username = username;
                }
            }

            await _next(context);
        }
    }
}
