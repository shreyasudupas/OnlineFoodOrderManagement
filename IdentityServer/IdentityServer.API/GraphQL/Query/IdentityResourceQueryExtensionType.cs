using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.QueryResolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class IdentityResourceQueryExtensionType
    {
        public async Task<List<IdentityResourceModel>> GetAllIdentityResource(
            [Service] IdenitityResourceQueryResolver idenitityResourceQueryResolver
            )
        {
            return await idenitityResourceQueryResolver.GetAllIdentityResource();
        }

        public async Task<IdentityResourceModel> GetIdentityResourceById(int id,
            [Service] IdenitityResourceQueryResolver idenitityResourceQueryResolver
            )
        {
            return await idenitityResourceQueryResolver.GetIdentityResourceById(id);
        }
    }
}
