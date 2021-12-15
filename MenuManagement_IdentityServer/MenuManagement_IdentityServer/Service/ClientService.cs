using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
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

            var Client = await ClientDbContext.Clients.Where(c => c.ClientId == ClientId).FirstOrDefaultAsync();
            if(Client != null)
            {
                result.status = CrudEnumStatus.success;

                result.ClientId = Client.ClientId;
                result.AccessTokenLifetime = Client.AccessTokenLifetime;
                result.Description = Client.Description;
                result.ClientName = Client.ClientName;
                result.RequireClientSecret = Client.RequireClientSecret;
                result.RequireConsent = Client.RequireConsent;
                result.CreatedDate = Client.Created;
            }
            else
            {
                result.status = CrudEnumStatus.failure;
                result.ErrorDescription.Add("Client not present");
            }
            return result;
        }
    }
}
