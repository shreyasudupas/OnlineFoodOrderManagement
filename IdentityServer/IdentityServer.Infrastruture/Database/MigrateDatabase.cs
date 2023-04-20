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
                    if (!context.Database.IsInMemory())
                    {
                        context.Database.Migrate();
                    }

                    AddMasterUsers(UserManager, context);

                    AddMasterClaims(context);
                    AddMasterRoles(RoleManager, context);
                    AddMasterUserClaims(UserManager, context);
                    MapUsersToRole(UserManager, context);

                    MapAddressStateLocation(context);

                    AddVendorMapping(context);
                }
            }
        }

        public static void MapAddressStateLocation(ApplicationDbContext context)
        {
            //States master values
            if (!context.States.Any() && !context.Cities.Any() && !context.LocationAreas.Any())
            {
                var states = new List<State>()
                            {
                                new State {
                                    Name="Karnataka",
                                    Cities = new List<City>
                                    {
                                        new City
                                        {
                                            Name="Bengaluru",
                                            Areas = new List<LocationArea>
                                            {
                                                new LocationArea { AreaName="Kathreguppe" },
                                                new LocationArea { AreaName="JP Nagar" },
                                                new LocationArea { AreaName="Jayanagar" },
                                                new LocationArea { AreaName="Uttrahalli" },
                                                new LocationArea { AreaName="Banashankari 2nd Stage" }
                                            }
                                        }
                                    }
                                },
                                new State {
                                    Name="Maharastra" ,
                                    Cities = new List<City>
                                    {
                                        new City
                                        {
                                            Name="Mumbai",
                                            Areas = new List<LocationArea>
                                            {
                                                new LocationArea { AreaName="Kalwa" },
                                                new LocationArea { AreaName="Thane" },
                                                new LocationArea { AreaName="Kurla" },
                                                new LocationArea { AreaName="Juhu" }
                                            }
                                        }
                                    }
                                },
                                new State {
                                    Name="Tamil Nadu",
                                    Cities = new List<City>
                                    {
                                        new City
                                        {
                                            Name="Chennai",
                                            Areas = new List<LocationArea>
                                            {
                                                new LocationArea { AreaName="RR Nagar" }
                                            }
                                        }
                                    }
                                }
                            };

                context.States.AddRange(states);

                context.SaveChanges();
            }
        }

        private static void MapUsersToRole(UserManager<ApplicationUser> UserManager, ApplicationDbContext context)
        {
            //Map users with Role
            if (!context.UserRoles.Any())
            {
                var users = context.Users.ToList();
                var roles = context.Roles.ToList();

                var adminRole = roles.Where(r => r.Name == "admin").FirstOrDefault();
                var userRole = roles.Where(r => r.Name == "user").FirstOrDefault();
                var vendorRole = roles.Where(r => r.Name == "vendor").FirstOrDefault();

                if (users.Count > 0 && roles.Count > 0)
                {
                    UserManager.AddToRoleAsync(users.Where(x=>x.UserName == "admin").First(), adminRole.Name).GetAwaiter().GetResult();
                    UserManager.AddToRoleAsync(users.Where(x => x.UserName == "user").First(), userRole.Name).GetAwaiter().GetResult();
                    UserManager.AddToRoleAsync(users.Where(x => x.UserName == "vendor").First(), vendorRole.Name).GetAwaiter().GetResult();
                }
                context.SaveChanges();
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
                    //var adminClaimRole = new Claim(GetClaimType, "admin");
                    var adminClaimEmail = new Claim("email", "admin@test.com");
                    var adminClaimUserName = new Claim("username", "admin"); //username same as role
                    var adminRoleClaim = new Claim("role", "admin");
                    var adminEnabledClaim = new Claim("enabled", "True");

                    var users = context.Users.ToList();

                    var adminUser = users.Where(x => x.UserName == "admin").FirstOrDefault();
                   // var result1 = UserManager.AddClaimAsync(users[0], adminClaimRole).GetAwaiter().GetResult();
                    var result2 = UserManager.AddClaimAsync(adminUser, adminClaimEmail).GetAwaiter().GetResult();
                    var result3 = UserManager.AddClaimAsync(adminUser, adminClaimUserName).GetAwaiter().GetResult();
                    var result4 = UserManager.AddClaimAsync(adminUser, adminRoleClaim).GetAwaiter().GetResult();
                    var result12 = UserManager.AddClaimAsync(adminUser, adminEnabledClaim).GetAwaiter().GetResult();

                    //for local users
                    //var userClaimRole = new Claim(GetClaimType, "appUser");
                    var normalUser = users.Where(x => x.UserName == "user").FirstOrDefault();
                    var userClaimEmail = new Claim("email", "user@test.com");
                    var userClaimUserName = new Claim("username", "user"); //username same as role
                    var userRoleClaim = new Claim("role", "user");

                    //var result5 = UserManager.AddClaimAsync(users[1], userClaimRole).GetAwaiter().GetResult();
                    var result6 = UserManager.AddClaimAsync(normalUser, userClaimEmail).GetAwaiter().GetResult();
                    var result7 = UserManager.AddClaimAsync(normalUser, userClaimUserName).GetAwaiter().GetResult();
                    var result8 = UserManager.AddClaimAsync(normalUser, userRoleClaim).GetAwaiter().GetResult();

                    //for vendor
                    var vendorUser = users.Where(x => x.UserName == "vendor").FirstOrDefault();
                    var vendorClaimEmail = new Claim("email", "vendor@test.com");
                    var vendorClaimUserName = new Claim("username", "vendor"); //username same as role
                    var vendorRoleClaim = new Claim("role", "vendor");

                    //var result5 = UserManager.AddClaimAsync(users[1], userClaimRole).GetAwaiter().GetResult();
                    var result9 = UserManager.AddClaimAsync(vendorUser, vendorClaimEmail).GetAwaiter().GetResult();
                    var result10 = UserManager.AddClaimAsync(vendorUser, vendorClaimUserName).GetAwaiter().GetResult();
                    var result11 = UserManager.AddClaimAsync(vendorUser, vendorRoleClaim).GetAwaiter().GetResult();
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
                                new IdentityRole { Name="user"},
                                new IdentityRole { Name="vendor"}
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
                                        FullAddress = "sample admin address 1, sample address 1",
                                        City = "Bengaluru",
                                        State = "Karnataka",
                                        Area="Kathreguppe",
                                        IsActive = true
                                    }
                                },
                    //IsAdmin = true,
                    Enabled=true,
                    UserType = IdenitityServer.Core.Domain.Enums.UserTypeEnum.Admin,
                    CreatedDate = DateTime.Now
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
                                        FullAddress = "sample user address 2, sample address 2",
                                        City = "Bengaluru",
                                        State = "Karnataka",
                                        Area="JP Nagar",
                                        IsActive = true
                                    }
                                },
                    //IsAdmin = false,
                    Enabled = true,
                    UserType = IdenitityServer.Core.Domain.Enums.UserTypeEnum.User,
                    CreatedDate = DateTime.Now
                };

                var resultTwo = UserManager.CreateAsync(userTwo, "password").GetAwaiter().GetResult();

                var userThree = new ApplicationUser
                {
                    UserName = "vendor",
                    Email = "vendor@test.com",
                    Address = new List<UserAddress>
                                {
                                    new UserAddress
                                    {
                                        FullAddress = "sample user address 3, sample address 3",
                                        City = "Bengaluru",
                                        State = "Karnataka",
                                        Area="JP Nagar",
                                        IsActive = true
                                    }
                                },
                    //IsAdmin = false,
                    Enabled=true,
                    UserType = IdenitityServer.Core.Domain.Enums.UserTypeEnum.Vendor,
                    CreatedDate = DateTime.Now
                };

                var resultThree = UserManager.CreateAsync(userThree, "password").GetAwaiter().GetResult();

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

        private static void AddVendorMapping(ApplicationDbContext context)
        {
            if(!context.VendorUserIdMappings.Any())
            {
                var vendorUser = context.Users.Where(x => x.UserName == "vendor").FirstOrDefault();

                if(vendorUser != null)
                {
                    context.VendorUserIdMappings.Add(new VendorUserIdMapping
                    {
                        UserId = vendorUser.Id,
                        Enabled = false
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
