using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.MutationResolver;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class ApiResourceMutationExtensionType
    {
        public async Task<ApiResourceModel> AddApiResource(
            ApiResourceModel apiResourceModel,
            [Service] ApiResourceMutationResolver apiResourceMutationResolver
            )
        {
            return await apiResourceMutationResolver.AddApiResource(apiResourceModel);
        }

        public async Task<ApiResourceModel> DeleteApiResourcesById(
            int id,
            [Service] ApiResourceMutationResolver apiResourceMutationResolver
            )
        {
            return await apiResourceMutationResolver.DeleteApiResourceById(id);
        }
    }
}
