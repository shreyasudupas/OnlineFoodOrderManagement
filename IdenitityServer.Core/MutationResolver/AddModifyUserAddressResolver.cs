using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class AddModifyUserAddressResolver
    {
        public readonly IUserService _userService;

        public AddModifyUserAddressResolver(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserProfileAddress> AddModifyUserAddress(string UserId,UserProfileAddress userProfileAddress)
        {
            var result = await _userService.AddModifyUserAddress(UserId, userProfileAddress);
            return result;
        }
    }
}
