using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility.UserInfo
{
    public class GetUserInfoByUserNameQuery  : IRequest<UserProfile>
    {
        public string UserName { get; set; }
    }

    public class GetUserInfoByUserNameQueryHandler : IRequestHandler<GetUserInfoByUserNameQuery, UserProfile>
    {
        private readonly IUserService _userService;

        public GetUserInfoByUserNameQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserProfile> Handle(GetUserInfoByUserNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserInfoByUserName(request.UserName);
            return result;
        }
    }
}
