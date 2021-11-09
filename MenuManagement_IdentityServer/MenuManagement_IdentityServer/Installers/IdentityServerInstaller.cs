using MenuManagement_IdentityServer.Configurations;
using MenuManagement_IdentityServer.Data;
using MenuManagement_IdentityServer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MenuManagement_IdentityServer.Installers
{
    public class IdentityServerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = configuration.GetConnectionString("sqlConnection");

            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(migrationAssembly));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(config=>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Authorization/Login";
                config.LogoutPath = "/Authorization/Logout";
            });

            services.AddIdentityServer()
                    .AddAspNetIdentity<ApplicationUser>()
                    .AddInMemoryClients(IdentityServer_Config.GetClients())
                    .AddInMemoryIdentityResources(IdentityServer_Config.GetIdentityResources())
                    .AddInMemoryApiResources(IdentityServer_Config.GetApiResources())
                    .AddInMemoryApiScopes(IdentityServer_Config.GetApiScopes())
                    //.AddDeveloperSigningCredential()
                    //.AddConfigurationStore(opt =>
                    //{
                    //    opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                    //        sql => sql.MigrationsAssembly(migrationAssembly));
                    //})
                    //.AddOperationalStore(opt =>
                    //{
                    //    opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                    //        sql => sql.MigrationsAssembly(migrationAssembly));
                    //})
                    //.AddTestUsers(IdentityServer_Config.GetTestUsers())
                    ;
                    
        }
    }
}
