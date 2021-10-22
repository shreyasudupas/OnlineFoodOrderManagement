using MenuOrder.MicroService.Features.MenuOrderFeature.Commands;
using MenuOrder.MicroService.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace MenuOrder.MicroService.SeriviceInstallers
{
    public class HttpClientFactoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Create the retry policy we want
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError() // HttpRequestException, 5XX and 408
                                        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            services.AddHttpClient("Basket MicroService", config=> {
                config.BaseAddress = new Uri(configuration["BaseApiUrls:BasketBaseUrl"]);
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
