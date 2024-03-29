﻿using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using MenuManagement_IdentityServer.Configurations;
using MenuManagement_IdentityServer.Data;
using MenuManagement_IdentityServer.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static IdentityModel.JwtClaimTypes;

namespace MenuManagement_IdentityServer.Extension
{
    public static class MigrationManager
    {
        public static IHost MigrateConfigurationDbContextDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    var web = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                    try
                    {
                        Console.WriteLine($">> Seeding for {web.EnvironmentName} started");

                        context.Database.Migrate();

                        if (!context.Clients.Any())
                        {
                            foreach (var client in IdentityServer_Config.GetClients())
                            {
                                context.Clients.Add(client.ToEntity());
                            }
                            context.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine($">> Data already Present in {web.EnvironmentName}");
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

                        MigrateApplicationDbContextDatabase(host);

                        Console.WriteLine($">> Seeding for {web.EnvironmentName} ended");
                        //use this to remove the clients
                        //var getAllClients = context.Clients.ToList();
                        //var getAllApiScopes = IdentityServer_Config.GetApiScopes();

                        //context.Clients.RemoveRange(getAllClients);
                        //context.ApiScopes.RemoveRange();
                        //context.ApiResources.RemoveRange();

                        //context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }
            
            return host;
        }

        public static IHost MigrateApplicationDbContextDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                //Run all migration
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                using var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                using var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    var transaction1 = context.Database.BeginTransaction();
                    try
                    {
                        //Fill the application user
                        if(!context.Users.Any())
                        {
                            //Add dummy admin user
                            var NewUser = new ApplicationUser
                            {
                                UserName = "admin",
                                Email = "admin@test.com",
                                Address = new List<UserAddress>
                                {
                                    new UserAddress
                                    {
                                        FullAddress = "sample address 1, sample address 1",
                                        City = "sample city",
                                        State = "sample State",
                                        IsActive = true
                                    }
                                },
                                IsAdmin = true
                            };
                            var Password = "password";

                            var result = UserManager.CreateAsync(NewUser,Password).GetAwaiter().GetResult();

                            context.SaveChanges();
                        }

                        //Fill Claims DropDown
                        if(!context.ClaimDropDowns.Any())
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

                        //Fill all roles
                        if(!context.Roles.Any())
                        {
                            var NewRoles = new List<IdentityRole>
                            {
                                new IdentityRole { Name = "admin"},
                                new IdentityRole { Name="appUser"}
                            };

                            foreach(var role in NewRoles)
                            {
                                var result = RoleManager.CreateAsync(role).GetAwaiter().GetResult();
                            }
                            context.SaveChanges();
                        }

                        //Add Claim for the user (role)
                        if (!context.UserClaims.Any())
                        {
                            var GetClaimType = context.ClaimDropDowns.Where(x => x.Name == "Role").Select(c => c.Value).FirstOrDefault();

                            if(GetClaimType!=null)
                            {
                                var userClaimRole = new Claim(GetClaimType, "admin");
                                var userClaimEmail = new Claim("email", "admin@test.com");
                                var userClaimUserName = new Claim("userName", "admin"); //username same as role

                                var GetUser = context.Users.FirstOrDefault();

                                var result1 = UserManager.AddClaimAsync(GetUser, userClaimRole).GetAwaiter().GetResult();
                                var result2 = UserManager.AddClaimAsync(GetUser, userClaimEmail).GetAwaiter().GetResult();
                                var result3 = UserManager.AddClaimAsync(GetUser, userClaimUserName).GetAwaiter().GetResult();
                            }
                            context.SaveChanges();
                        }

                        //Map newUser with admin Role
                        if (!context.UserRoles.Any())
                        {
                            var GetUser = context.Users.FirstOrDefault();
                            var GetAdminRole = context.Roles.FirstOrDefault();

                            if(GetUser!=null && GetAdminRole!= null)
                            {
                                var result = UserManager.AddToRoleAsync(GetUser, GetAdminRole.Name).GetAwaiter().GetResult();
                            }
                            context.SaveChanges();
                        }

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

                        transaction1.Commit();
                        

                    }catch(Exception ex)
                    {
                        transaction1.Rollback();
                        throw ex;
                    }
                }
            }

            return host;
        }
    }
}
