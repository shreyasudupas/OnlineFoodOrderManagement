using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.QueryResolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Query
{
    [ExtendObjectType("Query")]
    public class AddressQueryExtensionType
    {
        public async Task<List<State>> GetAddressList([Service] AddressQueryResolver addressQueryResolver)
        {
            return await addressQueryResolver.GetAddressList();
        }
    }
}
