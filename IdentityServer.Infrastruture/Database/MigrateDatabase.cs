using IdenitityServer.Core.Domain.DBModel;
using IdentityModel;
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
using System.Security.Claims;

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
                    AddMasterUsers(UserManager, context);

                    AddMasterClaims(context);
                    AddMasterRoles(RoleManager, context);
                    AddMasterUserClaims(UserManager, context);
                }
            }
        }

        private static void AddMasterUserClaims(UserManager<ApplicationUser> UserManager, ApplicationDbContext context)
        {
            //Add Claim for the user (role)
            if (!context.UserClaims.Any())
            {
                var GetClaimType = context.ClaimDropDowns.Where(x => x.Name == "Role").Select(c => c.Value).FirstOrDefault();

                if (GetClaimType != null)
                {
                    //for admin users
                    var adminClaimRole = new Claim(GetClaimType, "admin");
                    var adminClaimEmail = new Claim("email", "admin@test.com");
                    var adminClaimUserName = new Claim("username", "admin"); //username same as role

                    var users = context.Users.ToList();

                    var result1 = UserManager.AddClaimAsync(users[0], adminClaimRole).GetAwaiter().GetResult();
                    var result2 = UserManager.AddClaimAsync(users[0], adminClaimEmail).GetAwaiter().GetResult();
                    var result3 = UserManager.AddClaimAsync(users[0], adminClaimUserName).GetAwaiter().GetResult();

                    //for local users
                    var userClaimRole = new Claim(GetClaimType, "appUser");
                    var userClaimEmail = new Claim("email", "user@test.com");
                    var userClaimUserName = new Claim("username", "user"); //username same as role

                    var result4 = UserManager.AddClaimAsync(users[1], userClaimRole).GetAwaiter().GetResult();
                    var result5 = UserManager.AddClaimAsync(users[1], userClaimEmail).GetAwaiter().GetResult();
                    var result6 = UserManager.AddClaimAsync(users[1], userClaimUserName).GetAwaiter().GetResult();
                }
                context.SaveChanges();
            }
        }

        private static void AddMasterRoles(RoleManager<IdentityRole> RoleManager, ApplicationDbContext context)
        {
            //Fill all roles
            if (!context.Roles.Any())
            {
                var NewRoles = new List<IdentityRole>
                            {
                                new IdentityRole { Name = "admin"},
                                new IdentityRole { Name="appUser"}
                            };

                foreach (var role in NewRoles)
                {
                    var result = RoleManager.CreateAsync(role).GetAwaiter().GetResult();
                }
                context.SaveChanges();
            }
        }

        private static void AddMasterUsers(UserManager<ApplicationUser> UserManager, ApplicationDbContext context)
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

        private static void AddMasterClaims(ApplicationDbContext context)
        {
            //Fill Claims DropDown
            if (!context.ClaimDropDowns.Any())
            {
                var ListClaimsSelections = new List<ClaimDropDown>
                            {
                                new ClaimDropDown { Name = "Username", Value="userName"},
                                new ClaimDropDown { Name = "Email", Value=JwtClaimTypes.Email },
                                new ClaimDropDown { Name = "Role",Value=JwtClaimTypes.Role}
                            };

                context.ClaimDropDowns.AddRange(ListClaimsSelections);
                context.SaveChanges();
            }
        }
    }
}
