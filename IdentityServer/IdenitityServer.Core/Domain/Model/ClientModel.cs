using System;
using System.Collections.Generic;

namespace IdenitityServer.Core.Domain.Model
{
    public class ClientModel
    {
        public ClientModel()
        {
            AllowedGrantType = new List<string>();
            ClientSecrets = new List<ClientSecretModel>();
            AllowedScopes = new List<AllowedScopeModel>();
            RedirectUris = new List<RedirectUrlModel>();
            AllowedCorsOrigins = new List<AllowedCrosOriginModel>();
            PostLogoutRedirectUris = new List<PostLogoutRedirectUriModel>();
        }
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public List<string> AllowedGrantType { get; set; }
        public int AccessTokenLifetime { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool RequirePkce { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ClientSecretModel> ClientSecrets { get; set; }
        public List<AllowedScopeModel> AllowedScopes { get; set; }
        public List<RedirectUrlModel> RedirectUris { get; set; }
        public List<AllowedCrosOriginModel> AllowedCorsOrigins { get; set; }
        public List<PostLogoutRedirectUriModel> PostLogoutRedirectUris { get; set; }
    }

    public class ClientSecretModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string SecretValue { get; set; }
    }

    public class AllowedScopeModel
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ClientId { get; set; }
    }

    public class RedirectUrlModel
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }
        public int ClientId { get; set; }
    }
    public class AllowedCrosOriginModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ClientId { get; set; }
    }
    public class PostLogoutRedirectUriModel
    {
        public int Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public int ClientId { get; set; }
    }
}
