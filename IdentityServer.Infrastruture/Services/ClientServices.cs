using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
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

        public async Task<List<string>> SaveAllowedScopes(int clientId,List<string> selectedScopes)
        {
            var client = await _configurationDbContext.Clients.Include(x=>x.AllowedScopes)
                .Where(c => c.Id == clientId)
                .FirstOrDefaultAsync();

            if(client != null)
            {
                var newScope = new List<ClientScope>();

                //if existing ones are same
                if (client.AllowedScopes.Select(a => a.Scope).ToList().SequenceEqual(selectedScopes))
                {
                    return selectedScopes;
                }

                //new added scopes
                var tobeAddedScopes = selectedScopes.Except(client.AllowedScopes.Select(a => a.Scope)).ToArray();
                foreach (var tobeAddedScope in tobeAddedScopes)
                {
                    newScope.Add(new ClientScope { Scope = tobeAddedScope });
                    _logger.LogInformation($"Item with scope:{tobeAddedScope} addedd");
                }

                if (newScope.Count > 0)
                {
                    client.AllowedScopes.AddRange(newScope);
                }

                //Remove Old Scope
                var toBeRemovedScopes = client.AllowedScopes.Select(a => a.Scope).Except(selectedScopes);

                foreach (var toBeRemovedScope in toBeRemovedScopes.ToList())
                {
                    var item = client.AllowedScopes.Where(x => x.Scope == toBeRemovedScope).FirstOrDefault();
                    client.AllowedScopes.Remove(item);

                    _logger.LogInformation($"Item with scope:{toBeRemovedScope} removed");
                }

                _configurationDbContext.SaveChanges();

                return client.AllowedScopes.Select(x => x.Scope).ToList();
            }
            else
            {
                _logger.LogError($"client with ID {clientId} is not present in database");
                return null;
            }
        }

        public async Task<string> SaveAllowedGrants(int clientId, string selectedGrantTypes)
        {
            var client = await _configurationDbContext.Clients.Include(x => x.AllowedGrantTypes)
                .Where(c => c.Id == clientId)
                .FirstOrDefaultAsync();

            if (client != null)
            {
                var isExistsGrantType = client.AllowedGrantTypes.Any(g => g.GrantType == selectedGrantTypes);
                if(!isExistsGrantType)
                {
                    //add new grant type in our app 1:1 mapping is enough
                    client.AllowedGrantTypes = new List<ClientGrantType>
                    {
                        new ClientGrantType { GrantType = selectedGrantTypes }
                    };

                    _configurationDbContext.SaveChanges();
                    return client.AllowedGrantTypes.Select(x => x.GrantType).FirstOrDefault();
                }
                else
                {
                    _logger.LogInformation($"Grant Type {selectedGrantTypes} is already saved");
                    return selectedGrantTypes;
                }
            }
            else
            {
                _logger.LogError($"client with ID {clientId} is not present in database");
                return null;
            }
        }

        public async Task<ClientBasicInfo> SaveClient(ClientBasicInfo clientModel)
        {
            if(clientModel.Id == 0)
            {
                var client = new Client();

                client.ClientId = clientModel.ClientId;
                client.ClientName = clientModel.ClientName;
                client.Description = clientModel.Description;
                client.AccessTokenLifetime = clientModel.AccessTokenLifetime;
                client.RequireConsent = client.RequireConsent;
                client.RequirePkce = client.RequirePkce;
                client.Enabled = true;
                client.Created = DateTime.Now;

                _configurationDbContext.Clients.Add(client);
            }
            else
            {
                var client = await _configurationDbContext.Clients
                .Where(c => c.Id == clientModel.Id).FirstOrDefaultAsync();

                client.ClientId = clientModel.ClientId;
                client.ClientName = clientModel.ClientName;
                client.Description = clientModel.Description;
                client.AccessTokenLifetime = clientModel.AccessTokenLifetime;
                client.RequireConsent = client.RequireConsent;
                client.RequirePkce = client.RequirePkce;
            }

            _configurationDbContext.SaveChanges();

            return clientModel;
        }

        public async Task<ClientSecretModel> SaveClientSecret(ClientSecretModel clientSecretModel)
        {
            var client = await _configurationDbContext
                .Clients.Include(x=>x.ClientSecrets).Where(c => c.Id == clientSecretModel.ClientId).FirstOrDefaultAsync();

            if(client != null)
            {
                client.ClientSecrets.Add(new ClientSecret 
                {
                    Value = clientSecretModel.SecretValue
                });

                _configurationDbContext.SaveChanges();

                return client.ClientSecrets.Where(x=>x.Value == clientSecretModel.SecretValue)
                    .Select(s => new ClientSecretModel { Id = s.Id, ClientId = s.ClientId, SecretValue = s.Value }).FirstOrDefault();
            }
            else
            {
                _logger.LogError($"Client with Id {clientSecretModel.ClientId} not present in database");
                return new ClientSecretModel();
            }
        }

        public async Task<ClientSecretModel> DeleteClientSecret(ClientSecretModel clientSecretModel)
        {
            var client = await _configurationDbContext
                .Clients.Include(x => x.ClientSecrets).Where(c => c.Id == clientSecretModel.ClientId).FirstOrDefaultAsync();

            if (client != null)
            {
                var tobeDeleted = client.ClientSecrets.Where(c => c.Id == clientSecretModel.Id)
                    .FirstOrDefault();

                client.ClientSecrets.Remove(tobeDeleted);

                _configurationDbContext.SaveChanges();

                return clientSecretModel;
            }
            else
            {
                _logger.LogError($"Client with Id {clientSecretModel.ClientId} not present in database");
                return new ClientSecretModel();
            }
        }

        public async Task<AllowedCrosOriginModel> SaveClientAllowedCorsOrigin(AllowedCrosOriginModel allowedCrosOriginModel)
        {
            var client = await _configurationDbContext
                .Clients.Include(x => x.AllowedCorsOrigins).Where(c => c.Id == allowedCrosOriginModel.ClientId).FirstOrDefaultAsync();

            if (client != null)
            {
                client.AllowedCorsOrigins.Add(new ClientCorsOrigin
                {
                    Origin = allowedCrosOriginModel.Url
                });

                _configurationDbContext.SaveChanges();

                return client.AllowedCorsOrigins.Where(x=>x.Origin == allowedCrosOriginModel.Url)
                    .Select(s => new AllowedCrosOriginModel { Id = s.Id, ClientId = s.ClientId, Url = s.Origin }).FirstOrDefault();
            }
            else
            {
                _logger.LogError($"Client with Id {allowedCrosOriginModel.ClientId} not present in database");
                return new AllowedCrosOriginModel();
            }
        }

        public async Task<AllowedCrosOriginModel> DeleteClientAllowedCorsOrigin(AllowedCrosOriginModel allowedCrosOriginModel)
        {
            var client = await _configurationDbContext
                .Clients.Include(x => x.AllowedCorsOrigins).Where(c => c.Id == allowedCrosOriginModel.ClientId).FirstOrDefaultAsync();

            if (client != null)
            {
                var tobeDeleted = client.AllowedCorsOrigins.Where(c => c.Id == allowedCrosOriginModel.Id)
                    .FirstOrDefault();

                client.AllowedCorsOrigins.Remove(tobeDeleted);

                _configurationDbContext.SaveChanges();

                return allowedCrosOriginModel;
            }
            else
            {
                _logger.LogError($"Client with Id {allowedCrosOriginModel.ClientId} not present in database");
                return new AllowedCrosOriginModel();
            }
        }

        public async Task<RedirectUrlModel> SaveClientRedirectUrls(RedirectUrlModel redirectUrlModel)
        {
            var client = await _configurationDbContext
                .Clients.Include(x => x.RedirectUris).Where(c => c.Id == redirectUrlModel.ClientId).FirstOrDefaultAsync();

            if (client != null)
            {
                client.RedirectUris.Add(new ClientRedirectUri
                {
                    RedirectUri = redirectUrlModel.RedirectUri
                });

                _configurationDbContext.SaveChanges();

                return client.RedirectUris.Where(r=>r.RedirectUri == redirectUrlModel.RedirectUri)
                    .Select(s => new RedirectUrlModel { Id = s.Id, ClientId = s.ClientId, RedirectUri = s.RedirectUri }).FirstOrDefault();
            }
            else
            {
                _logger.LogError($"Client with Id {redirectUrlModel.ClientId} not present in database");
                return new RedirectUrlModel();
            }
        }

        public async Task<RedirectUrlModel> DeleteClientRedirectUrls(RedirectUrlModel redirectUrlModel)
        {
            var client = await _configurationDbContext
                .Clients.Include(x => x.RedirectUris).Where(c => c.Id == redirectUrlModel.ClientId).FirstOrDefaultAsync();

            if (client != null)
            {
                var tobeDeleted = client.RedirectUris.Where(c => c.Id == redirectUrlModel.Id)
                    .FirstOrDefault();

                client.RedirectUris.Remove(tobeDeleted);

                _configurationDbContext.SaveChanges();

                return redirectUrlModel;
            }
            else
            {
                _logger.LogError($"Client with Id {redirectUrlModel.ClientId} not present in database");
                return new RedirectUrlModel();
            }
        }
    }
}
