using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace MenuManagement_IdentityServer.Configurations
{
    public class IdentityServer_Config
    {
        public static IEnumerable<Client> GetClients() =>
        new Client[]
        {
            new Client
            {
                ClientId = "BasketServiceClient",
                ClientName = "Basket MicroService",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                ClientSecrets =
                {
                    new Secret("basketServiceMicroservice".Sha256())
                },
                AllowedScopes = { "openid", "userRole", "office", "profile" } //gets IdentityResource
            },
            new Client
            {
                ClientId = "AngularMenuClient",
                ClientName = "User Menu Client",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets =
                {
                    new Secret("angularUIClient".Sha256())
                },
                AllowedScopes = { "openid", "userRole", "office", "profile" },
                AllowedCorsOrigins = { "http://localhost:4200" },
                RequireClientSecret = false,
                RedirectUris =new List<string>{ "http://localhost:4200/signin-callback", "http://localhost:4200/assets/silent-callback.html" },
                PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                RequireConsent = false,
                AccessTokenLifetime = 600
            },
            new Client
            {
                ClientId = "client_id_mvc",
                ClientName = "Client MVC",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets =
                {
                    new Secret("client_secret_mvc".Sha256())
                },
                AllowedScopes = { "openid", "office", "profile" },
                RedirectUris =new List<string>{ "https://localhost:5006/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5006/Home/Index" },
                RequireConsent = false,
                AccessTokenLifetime = 3600
            },
            new Client
            {
                ClientId = "client_ids_UI",
                ClientName = "Client IDS UI",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets =
                {
                    new Secret("client_ids_UI".Sha256())
                },
                AllowedScopes = { "openid", "office", "profile" },
                RedirectUris =new List<string>{ "https://localhost:5005/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5005/Authorization/Login" },
                RequireConsent = false,
                AccessTokenLifetime = 3600
            }
        };
        public static IEnumerable<ApiScope> GetApiScopes() =>
         new ApiScope[]
         {
                new ApiScope("basketApi","Basket MicroService APIs")
                
         };
        public static IEnumerable<ApiResource> GetApiResources() =>
         new ApiResource[]
         {
             new ApiResource("basketApi","Basket MicroService APIs")
             {
                 Scopes = {"basketApi"}
             }
         };
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
         new IdentityResource[]
         {
             new IdentityResources.OpenId(),
             new IdentityResources.Profile(),
             new IdentityResource
             {
                 Name = "userRole",
                 UserClaims = {"role"}
             },
             new IdentityResource
             {
                 Name = "office",
                 UserClaims = {"office_number"}
             }
         };
        public static List<TestUser> GetTestUsers() =>
         new List<TestUser>
         {
             new TestUser
            {
                SubjectId = "1000",
                Username = "shreyas",
                Password = "udupa95",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Shreyas Udupa"),
                    new Claim(JwtClaimTypes.GivenName, "Shreyas"),
                    new Claim(JwtClaimTypes.FamilyName, "Udupa"),
                    new Claim(JwtClaimTypes.WebSite, "http://Ordermadi.com"),
                    new Claim(JwtClaimTypes.Email, "admin@test.com"),
                    new Claim("office_number","123545855"),
                    new Claim("role","app-user")
                }
            }
         };
    }
}
