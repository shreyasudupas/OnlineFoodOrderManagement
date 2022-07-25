using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class GetUserInformationResolver
    {
        private readonly IUserService _userService;

        public GetUserInformationResolver(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserProfile> GetUserInfo(string UserId)
        {
            var result = await _userService.GetUserInformationById(UserId);

            if(result!=null)
            {
                result = await _userService.GetUserClaims(result);

                result = await _userService.GetUserRoles(result);
            }

            return result;
        }
    }
}
