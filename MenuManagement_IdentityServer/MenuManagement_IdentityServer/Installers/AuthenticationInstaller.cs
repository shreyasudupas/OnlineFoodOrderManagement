using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MenuManagement_IdentityServer.Installers
{
    public class AuthenticationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddAuthentication(config=>
            //{
            //    config.DefaultScheme = "Cookies";
            //    config.DefaultChallengeScheme = "oidc";
            //})
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc",config=>       //will know to call the discovery document
            //    {
            //        config.SignInScheme = "Cookies";
            //        config.ClientId = "client_ids_UI";
            //        config.Authority = "https://localhost:5005/";
            //        config.ClientSecret = "angularUIClient";
            //        config.SaveTokens = true;
            //        config.ResponseType = "code";

            //        config.Scope.Add("openid");
            //        config.Scope.Add("profile");
            //        config.Scope.Add("office");
            //    })
            //    //.AddJwtBearer(jwtOptions =>
            //    //{
            //    //    jwtOptions.Authority = "https://localhost:5005/";
            //    //    jwtOptions.Audience = "AngularMenuClient";
            //    //    jwtOptions.SaveToken = true;
            //    //})
            //    ;
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt =>
                {
                    opt.Authority = "https://localhost:5005";
                    opt.Audience = "userIDSApi";
                });

            services.AddLocalApiAuthentication();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CommonRoleAccess",
                    policy => policy.RequireClaim(JwtClaimTypes.Role, "appUser","admin"));
            });
        }
    }
}
