using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Service
{
    public class ClientService : IClientService
    {
        private readonly ConfigurationDbContext ClientDbContext;
        private readonly IMapper _mapper;

        public ClientService(
            ConfigurationDbContext clientDbContext,
            IMapper mapper
            )
        {
            ClientDbContext = clientDbContext;
            _mapper = mapper;
        }

        public async Task<DisplayAllClients> GetAllClient()
        {
            DisplayAllClients displayAllClients = new DisplayAllClients();
            //get all Clients
            var Clients = await ClientDbContext.Clients.Include(grantType=>grantType.AllowedGrantTypes).ToListAsync();
           
            if(Clients.Count>0)
            {
                for(var client = 0; client<Clients.Count;client++)
                {
                    displayAllClients.Data.Add(new ClientData
                    {
                        ClientId = Clients[client].ClientId,
                        ClientName = Clients[client].ClientName,
                        Description = Clients[client].Description,
                        IsEnabled = Clients[client].Enabled,
                    });

                    //Add Grant Types 
                    displayAllClients.Data[client].AllowedGrantTypes.AddRange(Clients[client].AllowedGrantTypes);
                }
            }

            return displayAllClients;
        }

        public async Task<ClientViewModel> GetClientInformation(string ClientId)
        {
            ClientViewModel result = new ClientViewModel();

            if (ClientId != null)
            {
                var GetClientId = await ClientDbContext.Clients.Where(c => c.ClientId == ClientId).Select(x => x.Id).FirstOrDefaultAsync();
                var Client = await ClientDbContext.Clients
                                    .Include(re => re.RedirectUris)
                                    .Include(pos => pos.PostLogoutRedirectUris)
                                    .Include(ac => ac.AllowedCorsOrigins)
                                    .Include(s => s.ClientSecrets)
                                    .Include(gt => gt.AllowedGrantTypes)
                                    .Include(scope => scope.AllowedScopes)
                                    .Where(c => c.Id == GetClientId).FirstOrDefaultAsync();

                if (Client != null)
                {
                    result.status = CrudEnumStatus.success;

                    result.ClientId = Client.ClientId;
                    result.AccessTokenLifetime = Client.AccessTokenLifetime;
                    result.Description = Client.Description;
                    result.ClientName = Client.ClientName;
                    result.RequireClientSecret = Client.RequireClientSecret;
                    result.RequireConsent = Client.RequireConsent;
                    result.CreatedDate = Client.Created;
                    result.RequirePkce = Client.RequirePkce;

                    //Add all the scopes from GetApI
                    result.AllowedScopeList.AddRange(ClientDbContext.ApiScopes.Select(ac => new SelectListItem
                    {
                        Text = ac.DisplayName,
                        Value = ac.Name
                    }).ToList());

                    if (Client.RedirectUris != null)
                    {
                        foreach (var redirect in Client.RedirectUris)
                        {
                            result.RedirectUrls.Add(redirect.Id,redirect.RedirectUri);
                        }
                    }

                    if (Client.PostLogoutRedirectUris != null)
                    {
                        foreach (var postRedirect in Client.PostLogoutRedirectUris)
                        {
                            result.PostRedirectUrls.Add(postRedirect.Id,postRedirect.PostLogoutRedirectUri);
                        }
                    }

                    if (Client.AllowedCorsOrigins != null)
                    {
                        foreach (var allowedOrigin in Client.AllowedCorsOrigins)
                        {
                            result.AllowedCorsOrigins.Add(allowedOrigin.Id,allowedOrigin.Origin);
                        }
                    }

                    if(Client.ClientSecrets != null)
                    {
                        for (int i= 0;i < Client.ClientSecrets.Count;i++)
                        {
                            result.ClientSecrets.Add((i+1), Client.ClientSecrets[i].Description);
                        }
                    }

                    foreach(var type in Client.AllowedGrantTypes)
                    {
                        result.GrantTypesSelected.Add(type.GrantType);
                    }

                    foreach (var scope in Client.AllowedScopes)
                    {
                        result.AllowedScopeSelected.Add(scope.Scope);
                    }

                }
                else
                {
                    result.status = CrudEnumStatus.failure;
                    result.ErrorDescription.Add("Client not present");
                }
            }
            
            return result;
        }

        public async Task<ClientViewModel> SaveClientInformation(ClientViewModel model)
        {
            //remove previous errors
            model.ErrorDescription = new List<string>();

            try
            {
                var GetClientId = await ClientDbContext.Clients.Where(c => c.ClientId == model.ClientId).Select(c=>c.Id).FirstOrDefaultAsync();
                if (GetClientId > 0)
                {
                    var GetClient = await ClientDbContext.Clients
                        .Include(x => x.AllowedGrantTypes)
                        .Include(x => x.AllowedScopes)
                        .Where(x => x.Id == GetClientId).FirstOrDefaultAsync();

                    GetClient.ClientId = model.ClientId;
                    GetClient.RequireClientSecret = model.RequireClientSecret;
                    GetClient.ClientName = model.ClientName;
                    GetClient.Description = model.Description;
                    GetClient.RequireConsent = model.RequireConsent;
                    GetClient.AccessTokenLifetime = model.AccessTokenLifetime;

                    GetClient.AllowedGrantTypes = new List<ClientGrantType>();

                    //Adding Client Grant Type
                    ManageGrantType(model, model.ClientId);

                    //Manage allowed scope
                    ManageAllowedScope(model, model.ClientId);


                    model.status = CrudEnumStatus.success;

                }
                else
                {
                    IdentityServer4.EntityFramework.Entities.Client newClient = new IdentityServer4.EntityFramework.Entities.Client
                    {
                        ClientId = model.ClientId,
                        RequireClientSecret = model.RequireClientSecret,
                        ClientName = model.ClientName,
                        Description = model.Description,
                        RequireConsent = model.RequireConsent,
                        AccessTokenLifetime = model.AccessTokenLifetime,
                        Created = DateTime.Now
                    };

                    await ClientDbContext.Clients.AddAsync(newClient);

                    model.status = CrudEnumStatus.success;
                }

                await ClientDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                model.status = CrudEnumStatus.failure;
                model.ErrorDescription.Add(ex.Message);
            }
            return model;
        }

        public ClientSecretViewModel SaveClientSecret(ClientSecretViewModel model)
        {
            try
            {
                var Id = ClientDbContext.Clients.Where(c => c.ClientId == model.ClientId).Select(x => x.Id).FirstOrDefault();
                if(Id > 0)
                {
                    var clientDetails = ClientDbContext.Clients.Include(s => s.ClientSecrets).Where(x => x.Id == Id).FirstOrDefault();
                    clientDetails.ClientSecrets.Add(new ClientSecret
                    {
                        Description = model.Description,
                        Expiration = (string.IsNullOrEmpty(model.ExpirationDate))?null: Convert.ToDateTime(model.ExpirationDate),
                        Value = model.ClientSecret.Sha256()
                    });

                    //This is done is checkbox is not selected
                    clientDetails.RequireClientSecret = true;

                    ClientDbContext.SaveChanges();
                    model.status = CrudEnumStatus.success;
                }
                else
                {
                    model.ErrorDescription.Add($"No Client Found with Id {model.ClientId}");
                    model.status = CrudEnumStatus.failure;
                }
                
            }catch(Exception ex)
            {
                model.ErrorDescription.Add(ex.Message);
                model.status = CrudEnumStatus.failure;
            }
            return model;
        }

        public bool DeleteClientSecret(DeleteClientSecret ClientSecret)
        {
            var result = false;

            var client = ClientDbContext.Clients.Include(secret=>secret.ClientSecrets).Where(c => c.ClientId == ClientSecret.ClientId).FirstOrDefault();
            if(client != null)
            {
                var ClientSecrets = client.ClientSecrets.Find(x=>x.Id == ClientSecret.ClientSecretId);

                client.ClientSecrets.Remove(ClientSecrets);

                ClientDbContext.SaveChanges();
                result = true;

            }
            return result;
        }

        public RedirectUrlViewModel AddClientRedirect(RedirectUrlViewModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.ClientId))
                {
                    var ClientId = ClientDbContext.Clients.Where(c => c.ClientId == model.ClientId).Select(x => x.Id).FirstOrDefault();
                    if (ClientId > 0)
                    {
                        var ClientDetail = ClientDbContext.Clients.Include(red => red.RedirectUris).Where(x => x.Id == ClientId).FirstOrDefault();
                        //add redirect url
                        ClientDetail.RedirectUris.Add(new ClientRedirectUri
                        {
                            RedirectUri = model.ClientRedirectUrl
                        });

                        ClientDbContext.SaveChanges();
                        model.status = CrudEnumStatus.success;
                    }
                }
                else
                {
                    model.ErrorDescription.Add("Client Id is empty");
                }
            }
            catch(Exception ex)
            {
                model.ErrorDescription.Add(ex.Message);
                model.status = CrudEnumStatus.failure;
            }
            return model;
        }

        public bool DeleteClientRedirectUrl(DeleteRedirectUrl model)
        {
            var result = false;

            var client = ClientDbContext.Clients.Include(secret => secret.RedirectUris).Where(c => c.ClientId == model.ClientId).FirstOrDefault();
            if (client != null)
            {
                var ClientRedirectUri = client.RedirectUris.Find(x => x.Id == model.RedirectUrlId);

                client.RedirectUris.Remove(ClientRedirectUri);

                ClientDbContext.SaveChanges();
                result = true;

            }
            return result;
        }
        public AddCorsAllowedOriginViewModel AddClientOriginUrl(AddCorsAllowedOriginViewModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.ClientId))
                {
                    var clientId = ClientDbContext.Clients.Where(c => c.ClientId == model.ClientId).Select(c => c.Id).FirstOrDefault();
                    if(clientId>0)
                    {
                        var ClientDetails = ClientDbContext.Clients.Include(x => x.AllowedCorsOrigins).Where(x => x.Id == clientId).FirstOrDefault();

                        ClientDetails.AllowedCorsOrigins.Add(new ClientCorsOrigin
                        {
                            Origin = model.ClientOriginUrl
                        });

                        ClientDbContext.SaveChanges();
                        model.status = CrudEnumStatus.success;
                    }
                }
                else
                {
                    model.ErrorDescription.Add("Client Id is empty");
                    model.status = CrudEnumStatus.failure;
                }
            }catch(Exception ex)
            {
                model.ErrorDescription.Add(ex.Message);
                model.status = CrudEnumStatus.failure;
            }
            return model;
        }

        public bool DeleteClientAllowedOrigin(DeleteClientAllowedOrigin model)
        {
            var result = false;

            var client = ClientDbContext.Clients.Include(secret => secret.AllowedCorsOrigins).Where(c => c.ClientId == model.ClientId).FirstOrDefault();
            if (client != null)
            {
                var ClientAllowedOrigin = client.AllowedCorsOrigins.Find(x => x.Id == model.AllowedClientOriginId);

                client.AllowedCorsOrigins.Remove(ClientAllowedOrigin);

                ClientDbContext.SaveChanges();
                result = true;

            }
            return result;
        }
        public AddPostLogoutRedirectUriViewModel AddPostLogoutRedirectUrl(AddPostLogoutRedirectUriViewModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.ClientId))
                {
                    var clientId = ClientDbContext.Clients.Where(c => c.ClientId == model.ClientId).Select(c => c.Id).FirstOrDefault();
                    if (clientId > 0)
                    {
                        var ClientDetails = ClientDbContext.Clients.Include(x => x.PostLogoutRedirectUris).Where(x => x.Id == clientId).FirstOrDefault();

                        ClientDetails.PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri
                        {
                            PostLogoutRedirectUri = model.PostRedirectUri
                        });

                        ClientDbContext.SaveChanges();
                        model.status = CrudEnumStatus.success;
                    }
                }
                else
                {
                    model.ErrorDescription.Add("Client Id is empty");
                    model.status = CrudEnumStatus.failure;
                }
            }
            catch (Exception ex)
            {
                model.ErrorDescription.Add(ex.Message);
                model.status = CrudEnumStatus.failure;
            }
            return model;
        }
        public bool DeletePostLogoutUri(DeletePostLogoutRedirectUri model)
        {
            var result = false;

            var client = ClientDbContext.Clients.Include(secret => secret.PostLogoutRedirectUris).Where(c => c.ClientId == model.ClientId).FirstOrDefault();
            if (client != null)
            {
                var ClientPostLogoutUriToBeDeleted = client.PostLogoutRedirectUris.Find(x => x.Id == model.CLientPostLogoutId);

                client.PostLogoutRedirectUris.Remove(ClientPostLogoutUriToBeDeleted);

                ClientDbContext.SaveChanges();
                result = true;

            }
            return result;
        }

        public DeleteClientViewModel DeleteClient(string ClientId)
        {
            DeleteClientViewModel model = new DeleteClientViewModel();
            try
            {
                var GetClientId = ClientDbContext.Clients.Where(c => c.ClientId == ClientId).Select(c => c.Id).FirstOrDefault();
                if (GetClientId > 0)
                {
                    var ClientDetails = ClientDbContext.Clients.Where(x => x.Id == GetClientId).FirstOrDefault();

                    var result = ClientDbContext.Clients.Remove(ClientDetails);

                    ClientDbContext.SaveChanges();

                    model.status = CrudEnumStatus.success;
                }
                else
                {
                    model.status = CrudEnumStatus.failure;
                    model.ErrorDescription.Add("ClientId not found");
                }
            }
            catch(Exception ex)
            {
                model.status = CrudEnumStatus.failure;
                model.ErrorDescription.Add(ex.Message);
            }
            

            return model;
        }

        public void ManageAllowedScope(ClientViewModel model,string ClientId)
        {
            var Client = ClientDbContext.Clients.Include(x=>x.AllowedScopes).Where(c => c.ClientId == ClientId).FirstOrDefault();

            if(Client.AllowedScopes != null)
            {
                var ExistingScope = Client.AllowedScopes.Select(cas => cas.Scope).ToList();

                //if new scope is added to the existing list
                //var AddedNewScope = ExistingScope.Intersect(model.AllowedScopeSelected);
                var AddedNewScope = model.AllowedScopeSelected.Except(ExistingScope);

                if (AddedNewScope.Count() > 0)
                {
                    foreach (var scope in AddedNewScope)
                    {
                        Client.AllowedScopes.Add(new ClientScope
                        {
                            Scope = scope
                        });
                    }

                }

                //var RemovedScopes = model.AllowedScopeSelected.Intersect(ExistingScope.Where(x=> model.AllowedScopeSelected.Contains(x)));
                var RemovedScopes = ExistingScope.Except(model.AllowedScopeSelected);

                if (RemovedScopes.Count() > 0 )
                {
                    foreach(var ac in RemovedScopes)
                    {
                        //select the scopes
                        var addRemainingScopes = Client.AllowedScopes.Where(s => s.Scope == ac).FirstOrDefault();

                        Client.AllowedScopes.Remove(addRemainingScopes);
                    }
                    
                }

                ClientDbContext.SaveChanges();
            }
        }

        public void ManageGrantType(ClientViewModel model, string ClientId)
        {
            var Client = ClientDbContext.Clients.Include(x => x.AllowedGrantTypes).Where(c => c.ClientId == ClientId).FirstOrDefault();

            if (Client.AllowedGrantTypes != null)
            {
                var ExistingScope = Client.AllowedGrantTypes.Select(cas => cas.GrantType).ToList();

                //if new scope is added to the existing list
                //var AddedNewScope = ExistingScope.Intersect(model.AllowedScopeSelected);
                var AddedNewGrantType = model.GrantTypesSelected.Except(ExistingScope);

                if (AddedNewGrantType.Count() > 0)
                {
                    foreach (var grant in AddedNewGrantType)
                    {
                        Client.AllowedGrantTypes.Add(new ClientGrantType
                        {
                            GrantType = grant
                        });
                    }

                }

                //var RemovedScopes = model.AllowedScopeSelected.Intersect(ExistingScope.Where(x=> model.AllowedScopeSelected.Contains(x)));
                var RemovedGrantType = ExistingScope.Except(model.GrantTypesSelected);

                if (RemovedGrantType.Count() > 0)
                {
                    foreach (var grant in RemovedGrantType)
                    {
                        //select the scopes
                        var getGrantTypeToBeRemoved = Client.AllowedGrantTypes.Where(s => s.GrantType == grant).FirstOrDefault();

                        Client.AllowedGrantTypes.Remove(getGrantTypeToBeRemoved);
                    }

                }

                ClientDbContext.SaveChanges();
            }
        }

        public GetApiScopesViewModel GetAllApiScopes()
        {
            var result = new GetApiScopesViewModel();
            var ApiScopes = ClientDbContext.ApiScopes.ToList();

            if(ApiScopes.Count > 0)
            {
                result.ApiScopes = _mapper.Map<List<GetApiScopeModel>>(ApiScopes);

            }

            return result;
        }

        public GetApiScopeModel GetApiScopeById(int? Id)
        {
            GetApiScopeModel model = new GetApiScopeModel();
            if (Id != null)
            {
                var GetApiScope = ClientDbContext.ApiScopes.Where(api => api.Id == Id).FirstOrDefault();

                model.Id = GetApiScope.Id;
                model.Name = GetApiScope.Name;
                model.DisplayName = GetApiScope.DisplayName;
                model.Description = GetApiScope.Description;

            }
            return model;
        }

        public GetApiScopeModel ManageApiScope(GetApiScopeModel model)
        {
            var GetApiScope = ClientDbContext.ApiScopes.Where(api => api.Id == model.Id).FirstOrDefault();

            if(GetApiScope != null)
            {
                //Edit
                GetApiScope.Name = model.Name;
                GetApiScope.DisplayName = model.DisplayName;
                GetApiScope.Description = model.Description;

                ClientDbContext.SaveChanges();
            }
            else
            {
                //Add new Item
                ClientDbContext.ApiScopes.Add(new IdentityServer4.EntityFramework.Entities.ApiScope
                {
                    Name = model.Name,
                    DisplayName = model.DisplayName,
                    Description = model.Description
                });

                ClientDbContext.SaveChanges();
            }

            return model;
        }
    }
}
