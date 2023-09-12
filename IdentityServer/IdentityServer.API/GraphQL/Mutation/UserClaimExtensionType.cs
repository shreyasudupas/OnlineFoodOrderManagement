using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.Model;
using IdenitityServer.Core.MutationResolver;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class UserClaimExtensionType
    {
        public async Task<UserClaimModel> AddUserClaim(UserClaimModel userClaimModel, 
            [Service] UserClaimMutationResolver userClaimMutationResolver)
        {
            return await userClaimMutationResolver.AddUserClaimsBasedOnUserId(userClaimModel);
        }

        public async Task<UserClaimModel> ModifyUserClaim(UserClaimModel userClaimModel,
            [Service] UserClaimMutationResolver userClaimMutationResolver)
        {
            return await userClaimMutationResolver.ModifyUserClaimsBasedOnUserId(userClaimModel);
        }
    }
}
