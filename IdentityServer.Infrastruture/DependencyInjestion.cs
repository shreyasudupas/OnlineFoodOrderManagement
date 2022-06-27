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

            services.AddIdentity<ApplicationUser, IdentityRole>(config=> 
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options=> 
            {
                options.UserInteraction.LoginUrl = "http://localhost:3000/login";
                options.UserInteraction.ErrorUrl = "http://localhost:3000/error";
                options.UserInteraction.LogoutUrl = "http://localhost:3000/logout";
            })
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

            

            //Register services
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
