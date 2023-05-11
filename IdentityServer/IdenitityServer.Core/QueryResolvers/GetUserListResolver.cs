using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class GetUserListResolver
    {
        private readonly IUserService _userService;

        public GetUserListResolver(IUserService userService)
        {
            _userService = userService;
        }

        public Task<List<UserProfile>> GetUserList()
        {
            var result = _userService.GetUserList();
            return result;
        }
    }
}
