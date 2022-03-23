using BasketService.MicroService.Security.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BasketService.MicroService.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void InstallCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
