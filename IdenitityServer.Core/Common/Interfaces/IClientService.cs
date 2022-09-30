﻿using IdenitityServer.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientModel>> GetAllClients();
        Task<ClientModel> GetClientById(int clientId);
        Task<List<string>> SaveAllowedScopes(int clientId, List<string> scopes);
        Task<string> SaveAllowedGrants(int clientId, string selectedGrantTypes);
        Task<ClientBasicInfo> SaveClient(ClientBasicInfo clientModel);
    }
}
