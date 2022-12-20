using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.MutationResolver;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class ApiScopeMutationExtensionType
    {
        public async Task<ApiScopeModel> AddApiScope(ApiScopeModel apiScopeModel,
            [Service] ApiScopeMutationResolver apiScopeMutation)
        {
            return await apiScopeMutation.AddApiScope(apiScopeModel);
        }

        public async Task<ApiScopeModel> SaveApiScope(ApiScopeModel apiScopeModel,
            [Service] ApiScopeMutationResolver apiScopeMutation)
        {
            return await apiScopeMutation.SaveApiScope(apiScopeModel);
        }

        public async Task<ApiScopeModel> DeleteApiScope(int apiScopeId,
            [Service] ApiScopeMutationResolver apiScopeMutation)
        {
            return await apiScopeMutation.RemoveApiScope(apiScopeId);
        }
    }
}
