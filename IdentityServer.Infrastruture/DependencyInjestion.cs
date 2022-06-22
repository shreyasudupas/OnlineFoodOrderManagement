using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Infrastruture
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = services.AddIdentityServer()
                                .AddInMemoryClients(InMemoryConfiguration.Clients)
                                .AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources)
                                .AddInMemoryApiResources(InMemoryConfiguration.ApiResources)
                                .AddInMemoryApiScopes(InMemoryConfiguration.ApiScopes)
                                .AddTestUsers(InMemoryConfiguration.TestUsers)
                                .AddDeveloperSigningCredential();
            return services;
        }
    }
}
