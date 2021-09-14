using Microsoft.AspNetCore.Builder;
using Identity.MicroService.Security.Middleware;

namespace Identity.MicroService.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        //register the exception middleware
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void ConfigureAuthoriziationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GetUserMiddleware>();
        }

        public static void CallRequestLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }

}
