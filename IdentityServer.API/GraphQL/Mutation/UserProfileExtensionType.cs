using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.MutationResolver;
using IdentityServer.API.GraphQL.Middlewares;
using MenuOrder.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class UserProfileExtensionType
    {
        public async Task<RoleListResponse> AddRole(RoleListResponse newRole,
            [Service] MutationRoleResolver mutationRoleResolver)
        {
            return await mutationRoleResolver.AddRole(newRole);
        }

        public async Task<RoleListResponse> DeleteRole(string roleId,
            [Service] MutationRoleResolver mutationRoleResolver)
        {
            return await mutationRoleResolver.DeleteRole(roleId);
        }

        public RoleListResponse SaveRole(string roleId, string roleName,
            [Service] MutationRoleResolver mutationRoleResolver)
        {
            return mutationRoleResolver.SaveRole(new RoleListResponse { RoleId = roleId, RoleName = roleName });
        }

        [UseServiceScope]
        [UserId]
        public async Task<UserProfileAddress> AddModifyAddress(UserProfileAddress userProfileAddress,
            [Service] UserProfileResolver userProfileResolver,
            [Service] IProfileUser profileUser)
        {
            var result = await userProfileResolver.AddModifyUserAddress(profileUser.UserId, userProfileAddress);
            return result;
        }

        public async Task<UserProfile> AddUserProfilePicture(string id,IFile file,CancellationToken cancellationToken
            ,[Service] IWebHostEnvironment _webHostEnvironment
            )
        {
            using var stream = File.Create(System.IO.Path.Combine(_webHostEnvironment.WebRootPath + "\\images\\",  $"{id}.png"));
            return await Task.FromResult(new UserProfile());
        }
    }
}
