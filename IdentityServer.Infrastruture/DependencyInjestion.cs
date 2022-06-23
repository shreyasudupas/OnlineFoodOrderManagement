﻿using IdenitityServer.Core.Common.Interfaces;
using IdentityServer.Infrastruture.Database;
using IdentityServer.Infrastruture.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Infrastruture
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("MenuIdentityDB"));

            var builder = services.AddIdentityServer()
                                //.AddInMemoryClients(InMemoryConfiguration.Clients)
                                //.AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources)
                                //.AddInMemoryApiResources(InMemoryConfiguration.ApiResources)
                                //.AddInMemoryApiScopes(InMemoryConfiguration.ApiScopes)
                                //.AddTestUsers(InMemoryConfiguration.TestUsers)
                                .AddAspNetIdentity<ApplicationUser>()
                                .AddConfigurationStore(opt =>
                                {
                                    opt.ConfigureDbContext = c => c.UseInMemoryDatabase("SampleDB");
                                })
                                .AddOperationalStore(opt =>
                                {
                                    opt.ConfigureDbContext = c => c.UseInMemoryDatabase("SampleDB");
                                })
                                .AddDeveloperSigningCredential();

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //        .AddEntityFrameworkStores<ApplicationDbContext>();

            //Register services
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
