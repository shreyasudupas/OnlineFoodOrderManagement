using MenuInventory.MicroService.Security.Middleware;
using Microsoft.AspNetCore.Builder;

namespace MenuInventory.MicroService.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        //register the exception middleware
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

    }

}
