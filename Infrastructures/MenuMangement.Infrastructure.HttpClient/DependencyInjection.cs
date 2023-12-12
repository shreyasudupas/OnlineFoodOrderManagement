using MenuManagement.HttpClient.Domain.Interface;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.CartInformationClient;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.IdentityServer;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.InventoryClient;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.NotificationClient;
using MenuMangement.Infrastructure.HttpClient.Services.Payment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saga.Orchestrator.Core.Interfaces.Wrappers;

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

            services.AddHttpClient("IDSTokenClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:IdentityServerToken").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("InventoryClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:InventoryApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("NotificationClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:NotificationApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("OrderManagementClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:OrderManagementApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("CartManagementClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:CartManagementApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddTransient<Saga.Orchestrator.Core.Interfaces.Wrappers.IIdsHttpClientWrapper, IdsHttpClientWrapper>();
            services.AddTransient<INotificationClientWrapper, NotificationClientWrapper>();
            services.AddTransient<IInventoryClientWrapper, InventoryClientWrapper>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ICartInformationWrapper, CartInformationClientWrapper>();
        }
    }
}
