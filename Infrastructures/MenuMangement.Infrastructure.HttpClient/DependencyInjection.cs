using MenuManagement.HttpClient.Domain.Interface;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.IdentityServer;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.NotificationClient;
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
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:IdentityServer").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("VendorClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:VendorApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("NotificationClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:NotificationApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddTransient<IIdsHttpClientWrapper, IdsHttpClientWrapper>();
            services.AddTransient<INotificationClientWrapper, NotificationClientWrapper>();
        }
    }
}
