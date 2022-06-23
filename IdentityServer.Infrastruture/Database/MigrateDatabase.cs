using IdenitityServer.Core.Domain.DBModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Infrastruture.Database
{
    public static class MigrateDatabase
    {
        public static IHost UseMigration(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                if (!scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.IsInMemory())
                {
                    scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    if(!context.Database.IsInMemory())
                    {
                        context.Database.Migrate();
                    }
                    
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

                    //Seed ApplicationDBContext
                    SeedApplicationDbContext(host);
                }
            }
            return host;
        }

        private static void SeedApplicationDbContext(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                using var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    if (!context.Users.Any())
                    {
                        //Add dummy admin user
                        var userOne = new ApplicationUser
                        {
                            UserName = "admin",
                            Email = "admin@test.com",
                            Address = new List<UserAddress>
                                {
                                    new UserAddress
                                    {
                                        FullAddress = "sample address 1, sample address 1",
                                        City = "Bangalore",
                                        State = "Karnataka",
                                        IsActive = true
                                    }
                                },
                            IsAdmin = true
                        };

                        var result = UserManager.CreateAsync(userOne, "password").GetAwaiter().GetResult();

                        var userTwo = new ApplicationUser
                        {
                            UserName = "user",
                            Email = "user@test.com",
                            Address = new List<UserAddress>
                                {
                                    new UserAddress
                                    {
                                        FullAddress = "sample address 1, sample address 1",
                                        City = "Bangalore",
                                        State = "Karnataka",
                                        IsActive = true
                                    }
                                },
                            IsAdmin = false
                        };

                        var resultTwo = UserManager.CreateAsync(userTwo, "password").GetAwaiter().GetResult();

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
