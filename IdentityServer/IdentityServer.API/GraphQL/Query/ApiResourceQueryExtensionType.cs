using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.QueryResolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class ApiResourceQueryExtensionType
    {
        public async Task<List<ApiResourceModel>> GetAllApiResources(
            [Service] ApiResourceQueryResolver apiResourceQueryResolver
            )
        {
            return await apiResourceQueryResolver.GetAllApiResources();
        }

        public async Task<ApiResourceModel> GetApiResourcesById(
            int id,
            [Service] ApiResourceQueryResolver apiResourceQueryResolver
            )
        {
            return await apiResourceQueryResolver.GetApiResourceById(id);
        }
    }
}
