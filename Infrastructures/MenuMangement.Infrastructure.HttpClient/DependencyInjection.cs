using IdenitityServer.Core.Common.Interfaces;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.IdentityServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuMangement.Infrastructure.HttpClient
{
    public static class DependencyInjection
    {
        public static void AddInfrastrutureHttpClient(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpClient("IDSClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:NotificationApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("VendorClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:VendorApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddScoped<IIdsHttpClientWrapper, IdsHttpClientWrapper>();
        }
    }
}
