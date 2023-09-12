using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Model;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class UserClaimMutationResolver
    {
        private readonly IUserService _userService;

        public UserClaimMutationResolver(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserClaimModel> AddUserClaimsBasedOnUserId(UserClaimModel userClaimModel) 
        {
            return await _userService.AddUserClaimsBasedOnUserId(userClaimModel);
        }

        public async Task<UserClaimModel> ModifyUserClaimsBasedOnUserId(UserClaimModel userClaimModel)
        {
            return await _userService.ModifyUserClaimsBasedOnUserId(userClaimModel);
        }
    }
}
