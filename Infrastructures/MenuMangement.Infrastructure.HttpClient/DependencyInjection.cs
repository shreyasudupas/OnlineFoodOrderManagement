using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MenuMangement.HttpClient.Domain.Interfaces.GraphQl;
using MenuMangement.HttpClient.Domain.Interfaces.Services;
using MenuMangement.HttpClient.Domain.Interfaces.Wrappers;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.CartInformationClient;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.IdentityServer;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.IdsGraphQl;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.InventoryClient;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.NotificationClient;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.SignalRNotificationClient;
using MenuMangement.Infrastructure.HttpClient.ClientWrapper.SignalROrderClient;
using MenuMangement.Infrastructure.HttpClient.Services.Order;
using MenuMangement.Infrastructure.HttpClient.Services.Payment;
using MenuMangement.Infrastructure.HttpClient.Services.Vendor;
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

            services.AddHttpClient("VendorManagementClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:VendorManagementApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient("SignalRServiceClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("ExternalAPIs:SignalRServiceApi").Value);
                config.DefaultRequestHeaders.Clear();
            });

            services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(configuration.GetSection("ExternalAPIs:GraphQlClient").Value,
                new NewtonsoftJsonSerializer()));

            services.AddTransient<IIdsHttpClientWrapper, IdsHttpClientWrapper>();
            services.AddTransient<INotificationClientWrapper, NotificationClientWrapper>();
            services.AddTransient<IInventoryClientWrapper, InventoryClientWrapper>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ICartInformationWrapper, CartInformationClientWrapper>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IVendorSerivce, VendorService>();
            services.AddTransient<ISignalRNotificationClientWrapper, SignalRNotificationClientWrapper>();
            services.AddTransient<ISignalROrderClientWrapper,SignalROrderClientWrapper>();

            services.AddTransient<IGetUserRewardQuery, GetUserRewardQuery>();
        }
    }
}
