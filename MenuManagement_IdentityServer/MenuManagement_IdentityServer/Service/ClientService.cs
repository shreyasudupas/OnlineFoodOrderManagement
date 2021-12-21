using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
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
                            result.RedirectUrls.Add(redirect.RedirectUri);
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
                            result.AllowedCorsOrigins.Add(allowedOrigin.Origin);
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
                    Client newClient = new Client
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
    }
}
