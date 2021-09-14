using MenuOrder.MicroService.Security.Middleware;
using Microsoft.AspNetCore.Builder;

namespace MenuOrder.MicroService.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void AddGlobalExceptionMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
