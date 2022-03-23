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
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",
                ClientSecrets = new [] { new Secret("supersecret".Sha512()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {  "basketApi",IdentityServerConstants.LocalApi.ScopeName } //cannot have openId for client credential flow
             },
            new Client
            {
                ClientId = "interactive",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequireConsent = false,
                RequirePkce = true,
                RedirectUris = new List<string> { "http://localhost:4200/signin-callback" },
                PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                AllowedScopes = new List<string>{ 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName, //This is used to call API within the IDS server API when client that doesnt have cookie and can access API
                    "userIDSApi", //this is as audience,
                    "GetUserRole" //This is used to get role in id_token
                },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:4200",
                },
                AccessTokenLifetime = 86400,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                Claims = new List<ClientClaim>
                {
                    new ClientClaim(JwtClaimTypes.Role,"admin"),
                    new ClientClaim(JwtClaimTypes.Role,"appUser")
                }
            }

        };
        public static IEnumerable<ApiScope> GetApiScopes() =>
         new ApiScope[]
         {
                new ApiScope("basketApi","Basket MicroService API read"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
                new ApiScope("userIDSApi","User Controller API in IDS")
         };
        //For audience put in both ApiResource and GetApiScope so that in client machine can give audience as eg:basketApi
        public static IEnumerable<ApiResource> GetApiResources() =>
         new ApiResource[]
         {
             new ApiResource("basketApi","Basket MicroService APIs")
             {
                 Scopes = {"basketApi"}
             },
             //name,Description,claims
             new ApiResource("userIDSApi","User Controller API in IDS",new List<string>() { ClaimTypes.Role })
             {
                 Scopes = { "userIDSApi" }
             }
         };

        //An identity resource has meaning as long as it has a claim.Therefore there is a requirement that an identity resource must have minimum 1 claim in the claims list.
        //In the example below we define an identityresource with the scope name role having a custom claim called my_pet_name.
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
         new IdentityResource[]
         {
             new IdentityResources.OpenId(),
             new IdentityResources.Profile(),
             new IdentityResource("GetUserRole",
                 new List<string>
                 {
                     //claims
                     "role"
                 })
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
