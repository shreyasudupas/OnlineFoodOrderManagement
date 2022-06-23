using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace IdentityServer.Infrastruture.Database
{
    public static class MigrateDatabase
    {
        public static IHost UseMigration(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    context.Database.Migrate();
                    var web = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                    if (!context.Clients.Any())
                    {
                        foreach (var client in InMemoryConfiguration.Clients)
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($" Clients already Present in {web.EnvironmentName}");
                    }

                    if (!context.IdentityResources.Any())
                    {
                        foreach (var resource in InMemoryConfiguration.IdentityResources)
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($" IdenetityResources already Present in {web.EnvironmentName}");
                    }

                    if (!context.ApiScopes.Any())
                    {
                        foreach (var apiScope in InMemoryConfiguration.ApiScopes)
                        {
                            context.ApiScopes.Add(apiScope.ToEntity());
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($" ApiScopes already Present in {web.EnvironmentName}");
                    }

                    if (!context.ApiResources.Any())
                    {
                        foreach (var resource in InMemoryConfiguration.ApiResources)
                        {
                            context.ApiResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($" Api Resources already Present in {web.EnvironmentName}");
                    }
                }
            }
            return host;
        }
    }
}
