using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service.Interface;
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
                                    .Include(s=>s.ClientSecrets)
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
                            result.PostRedirectUrls.Add(postRedirect.PostLogoutRedirectUri);
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
                var GetClient = await ClientDbContext.Clients.Where(c => c.ClientId == model.ClientId).FirstOrDefaultAsync();
                if (GetClient != null)
                {
                    GetClient.ClientId = model.ClientId;
                    GetClient.RequireClientSecret = model.RequireClientSecret;
                    GetClient.ClientName = model.ClientName;
                    GetClient.Description = model.Description;
                    GetClient.RequireConsent = model.RequireConsent;
                    GetClient.AccessTokenLifetime = model.AccessTokenLifetime;

                    
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
    }
}
