using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class ClientServices : IClientService
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ILogger _logger;

        public ClientServices(ConfigurationDbContext configurationDbContext,
            ILogger<ClientServices> logger)
        {
            _configurationDbContext = configurationDbContext;
            _logger = logger;
        }

        public async Task<List<ClientModel>> GetAllClients()
        {
            var clients = await _configurationDbContext.Clients.Include(x=>x.AllowedGrantTypes).Select(c => new ClientModel
            {
                Id = c.Id,
                ClientId = c.ClientId,
                Description = c.Description,
                ClientName = c.ClientName,
                Enabled = c.Enabled,
                AllowedGrantType = c.AllowedGrantTypes.Select(x=>x.GrantType).ToList()
            }).ToListAsync();

            return clients;
        }

        public async Task<ClientModel> GetClientById(int clientId)
        {
            var client = await _configurationDbContext.Clients
                .Include(x => x.AllowedGrantTypes)
                .Include(x=>x.ClientSecrets)
                .Include(x=>x.RedirectUris)
                .Include(x=>x.AllowedCorsOrigins)
                .Include(x=>x.PostLogoutRedirectUris)
                .Include(x=>x.AllowedScopes)
                .Where(c=>c.Id == clientId).Select(c => new ClientModel
                {
                    Id = c.Id,
                    ClientId = c.ClientId,
                    Description = c.Description,
                    ClientName = c.ClientName,
                    Enabled = c.Enabled,
                    AllowedGrantType = c.AllowedGrantTypes.Select(x => x.GrantType).ToList(),
                    ClientSecrets = c.ClientSecrets.Select(s=> new ClientSecretModel { Id = s.Id, ClientId = s.ClientId , SecretValue = s.Value }).ToList(),
                    AccessTokenLifetime = c.AccessTokenLifetime,
                    CreatedDate = c.Created,
                    AllowedScopes = c.AllowedScopes.Select(ac=> new AllowedScopeModel { Id = ac.Id , Scope = ac.Scope , ClientId = ac.ClientId }).ToList(),
                    RedirectUris = c.RedirectUris.Select(ru => new RedirectUrlModel { Id = ru.Id,RedirectUri = ru.RedirectUri,ClientId=ru.ClientId }).ToList(),
                    AllowedCorsOrigins = c.AllowedCorsOrigins.Select(ao=>new AllowedCrosOriginModel { Id = ao.Id , Url = ao.Origin , ClientId = ao.ClientId }).ToList(),
                    RequirePkce = c.RequirePkce,
                    RequireConsent = c.RequireConsent,
                    PostLogoutRedirectUris = c.PostLogoutRedirectUris.Select(pl=>new PostLogoutRedirectUriModel { Id = pl.Id , PostLogoutRedirectUri = pl.PostLogoutRedirectUri,ClientId = pl.ClientId }).ToList()
                }).FirstOrDefaultAsync();

            if(client != null)
                return client;
            else
            {
                _logger.LogError($"Client Id: {clientId} has no data in database");
                return null;
            }
        }
    }
}
