using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class UpdateUserInfoResolver
    {
        private readonly IUserService _userService;

        public UpdateUserInfoResolver(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> AddUserInformation(UserProfile user)
        {
            var result =await _userService.ModifyUserInformation(user);
            return result;
        }
    }
}
