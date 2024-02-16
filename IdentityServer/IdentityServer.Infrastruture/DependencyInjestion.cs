using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Common.Interfaces.HttpClient;
using IdenitityServer.Core.Common.Interfaces.Repository;
using IdentityServer.Infrastruture.Database;
using IdentityServer.Infrastruture.HttpClient;
using IdentityServer.Infrastruture.Repositories;
using IdentityServer.Infrastruture.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer.Infrastruture
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            

            services.AddIdentity<ApplicationUser, IdentityRole>(config=> 
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.React.SPA";
                config.LoginPath = "/Authentication/Login";
                config.LogoutPath = "/Authentication/Logout";
                config.AccessDeniedPath = "/Authentication/AccessDenied";
                config.ExpireTimeSpan = TimeSpan.FromMinutes(120);

                config.Cookie.HttpOnly = true;
                config.Cookie.SameSite = SameSiteMode.None;
                config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            var connectionString = configuration.GetConnectionString("sqlConnection");

            if(string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("MenuIdentityDB"));

                var builder = services.AddIdentityServer(
                    y =>
                    {
                        y.EmitStaticAudienceClaim = true;
                    }
                                //    options=> 
                                //{
                                //    options.UserInteraction.LoginUrl = "http://localhost:3000/login";
                                //    options.UserInteraction.ErrorUrl = "http://localhost:3000/error";
                                //    options.UserInteraction.LogoutUrl = "http://localhost:3000/logout";
                                //}
                                )
                                //.AddInMemoryClients(InMemoryConfiguration.Clients)
                                //.AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources)
                                //.AddInMemoryApiResources(InMemoryConfiguration.ApiResources)
                                //.AddInMemoryApiScopes(InMemoryConfiguration.ApiScopes)
                                //.AddTestUsers(InMemoryConfiguration.TestUsers)
                                .AddAspNetIdentity<ApplicationUser>()
                                .AddProfileService<CustomProfileService>() //This is used for adding custom claims
                                .AddConfigurationStore(opt =>
                                {
                                    opt.ConfigureDbContext = c => c.UseInMemoryDatabase("SampleDB");
                                })
                                .AddOperationalStore(opt =>
                                {
                                    opt.ConfigureDbContext = c => c.UseInMemoryDatabase("SampleDB");
                                })
                                .AddDeveloperSigningCredential();
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(builder =>
                {
                    builder.UseSqlServer(connectionString, sql =>
                                     sql.MigrationsAssembly("IdentityServer.Infrastruture"));
                });

                var builder = services.AddIdentityServer(
                                )
                                .AddAspNetIdentity<ApplicationUser>()
                                .AddProfileService<CustomProfileService>() //This is used for adding custom claims
                                .AddConfigurationStore(opt =>
                                {
                                    opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,sql=>
                                    sql.MigrationsAssembly("IdentityServer.Infrastruture"));
                                })
                                .AddOperationalStore(opt =>
                                {
                                    opt.ConfigureDbContext = c => c.UseSqlServer(connectionString, sql =>
                                     sql.MigrationsAssembly("IdentityServer.Infrastruture"));
                                })
                                .AddDeveloperSigningCredential();

            }


            services.AddLocalApiAuthentication();

            //Register services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUtilsService, UtilsService>();
            services.AddScoped<IUserService, UserServices>();
            services.AddScoped<IAdministrationService, AdministrationService>();
            services.AddScoped<IClientService,ClientServices>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IVendorUserMappingService, VendorUserMappingService>();

            services.AddScoped<IUserPointEventRepository, UserPointEventRepository>();


            services.AddHttpClient("ReverseGeoLocationClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("LocationSearchAPI:ReverseAPI").Value);
                config.DefaultRequestHeaders.Clear();
            });
            services.AddHttpClient("ForwardGeoLocationClient", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("LocationSearchAPI:ForwardGeoCoding").Value);
                config.DefaultRequestHeaders.Clear();
            });
            services.AddTransient<ILocationSearch, LocationSearch>();

            return services;
        }
    }
}
