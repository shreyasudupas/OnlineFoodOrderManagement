using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Infrastruture
{
    public static class InMemoryConfiguration
    {
        public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "admin.client",
                ClientName = "Client Credentials Client",
                Description= "This is for admin to access the token",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                //ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = new List<string> { 
                    "inventory.read", 
                    "inventory.write", 
                    "notification.read", 
                    "notification.write" 
                }
            },
            new Client
            {
                ClientId = "react-spa-identityServer-ui",
                ClientName = "React SPA IDS UI",
                ClientUri = "http://localhost:3000",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris = new List<string> { "http://localhost:3000/signin-callback" },
                PostLogoutRedirectUris = new List<string> { "http://localhost:3000/signout-callback" },
                AllowedScopes = new List<string>{
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName, //This is used to call API within the IDS server API when client that doesnt have cookie and can access API
                    "ids.read",
                    "ids.write"
                },
                AccessTokenLifetime = 86400,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:3000",
                }
            },
            new Client
            {
                ClientId = "menuAngularUI",
                ClientName = "Menu Angular UI",
                ClientUri = "http://localhost:4200",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris = new List<string> { "http://localhost:4200/signin-callback" },
                PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                AllowedScopes = new List<string>{
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName,
                    "inventory.read",
                    "ids.read",
                    "inventory.write",
                    "notification.read",
                    "notification.write"
                },
                AccessTokenLifetime = 86400,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:4200",
                }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            //new ApiScope("myApi.read"),
            //new ApiScope("myApi.write"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            new ApiScope("idsApi.read","IDS API Read"),
            new ApiScope("idsApi.write","IDS API Write"),
            new ApiScope("inventory.read","Inventory API Read"),
            new ApiScope("inventory.write","Inventory API Write"),
            new ApiScope("notification.read","Notification API Read"),
            new ApiScope("notification.write","Notification API Write")
        };

        public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1144",
                Username = "mukesh",
                Password = "mukesh",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Mukesh Murugan"),
                    new Claim(JwtClaimTypes.GivenName, "Mukesh"),
                    new Claim(JwtClaimTypes.FamilyName, "Murugan"),
                    new Claim(JwtClaimTypes.WebSite, "http://codewithmukesh.com"),
                }
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("ids","Identity Server API")
            {
                Scopes = new List<string>{ "idsApi.read", "idsApi.write" },
            },
            new ApiResource("inventory","Inventory Microservice API")
            {
                Scopes = new List<string>{ "inventory.read", "inventory.write" },
            },
            new ApiResource("notification","Notification Microservice API")
            {
                Scopes = new List<string>{ "notification.read", "notification.write" },
            }
        };
    }
}
