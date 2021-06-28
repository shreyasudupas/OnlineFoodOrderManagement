using Microsoft.AspNetCore.Builder;
using User.MicroService.Security.Middleware;

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
