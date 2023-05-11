using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.QueryResolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class ClientQueryExtensionType
    {
        public async Task<List<ClientModel>> GetClientsInformation(
            [Service] ClientQueryResolver clientQueryResolver)
        {
            return await clientQueryResolver.GetAllClients();
        }

        public async Task<ClientModel> GetClientById(int clientId,
            [Service] ClientQueryResolver clientQueryResolver)
        {
            return await clientQueryResolver.GetClientById(clientId);
        }
    }
}
