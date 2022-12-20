using HotChocolate.Resolvers;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Middlewares
{
    public class UserIdMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private StringValues userIdValue;
        private const string UserId_Header = "userId";

        public UserIdMiddleware(FieldDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(IMiddlewareContext context, IProfileUser profile
            )
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var request = httpContext.Request.Headers;

                if(request.TryGetValue(UserId_Header,out userIdValue))
                {
                    profile.UserId = userIdValue.ToString();
                }
                
            }
            await _next(context);
        }
    }
}
