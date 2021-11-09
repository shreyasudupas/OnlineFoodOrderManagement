using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using MenuManagement_IdentityServer.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace MenuManagement_IdentityServer.Extension
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();

                        if (!context.Clients.Any())
                        {
                            foreach (var client in IdentityServer_Config.GetClients())
                            {
                                context.Clients.Add(client.ToEntity());
                            }
                            context.SaveChanges();
                        }

                        if (!context.IdentityResources.Any())
                        {
                            foreach (var resource in IdentityServer_Config.GetIdentityResources())
                            {
                                context.IdentityResources.Add(resource.ToEntity());
                            }
                            context.SaveChanges();
                        }

                        if (!context.ApiScopes.Any())
                        {
                            foreach (var apiScope in IdentityServer_Config.GetApiScopes())
                            {
                                context.ApiScopes.Add(apiScope.ToEntity());
                            }

                            context.SaveChanges();
                        }

                        if (!context.ApiResources.Any())
                        {
                            foreach (var resource in IdentityServer_Config.GetApiResources())
                            {
                                context.ApiResources.Add(resource.ToEntity());
                            }
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
