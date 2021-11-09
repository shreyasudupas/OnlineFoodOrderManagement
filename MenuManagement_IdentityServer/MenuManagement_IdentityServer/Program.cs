//using MenuManagement_IdentityServer.Extension;
using MenuManagement_IdentityServer.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();

            //Register In Memory Identity User
            //using (var scope = builder.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //    var user = new IdentityUser("shreyas@gmail.com");
            //    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
            //}
                //builder.MigrateDatabase().Run();
            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
