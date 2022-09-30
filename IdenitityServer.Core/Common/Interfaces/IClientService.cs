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
        Task<List<string>> SaveAllowedGrants(int clientId, List<string> selectedGrantTypes);
        Task<ClientBasicInfo> SaveClient(ClientBasicInfo clientModel);
    }
}