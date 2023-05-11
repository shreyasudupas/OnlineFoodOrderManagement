using MenuOrder.Shared.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace MenuOrder.Shared.Extension
{
    public static class ExtensionMiddleware
    {
        public static void RegisterSharedMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionMiddleware>();
            applicationBuilder.UseMiddleware<UserMiddleware>();
        }
    }
}
