using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.QueryResolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class ApiScopeQueryExtensionType
    {
        public async Task<List<ApiScopeModel>> GetApiScopes(
            [Service] ApiScopeQueryResolver apiScopeQuery)
        {
            return await apiScopeQuery.GetAllApiScopes();
        }

        public async Task<ApiScopeModel> GetApiScopeById(int id,
            [Service] ApiScopeQueryResolver apiScopeQuery)
        {
            return await apiScopeQuery.GetApiScopeById(id);
        }
    }
}
