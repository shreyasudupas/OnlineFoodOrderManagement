using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.MutationResolver;
using IdentityServer.API.GraphQL.Middlewares;
using MenuOrder.Shared.Services.Interfaces;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class AddModifyUserAddressExtensionType
    {
        //[Authorize(Policy = LocalApi.PolicyName)]
        //[Authorize(Policy = "isAdmin")]
        [UseServiceScope]
        [UserId]
        public async Task<UserProfileAddress> AddModifyAddress(UserProfileAddress userProfileAddress,
            [Service] AddModifyUserAddressResolver userAddressResolver,
            [Service] IProfileUser profileUser)
        {
            var result = await userAddressResolver.AddModifyUserAddress(profileUser.UserId, userProfileAddress);
            return result;
        }
    }
}
