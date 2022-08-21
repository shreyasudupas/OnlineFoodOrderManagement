using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
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

        public async Task<SaveUserResponse> AddUserInformation(UserProfile user)
        {
            var response = new SaveUserResponse();
            response.Result = await _userService.ModifyUserInformation(user);
            return response;
        }
    }
}
