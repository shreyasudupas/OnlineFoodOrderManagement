using Identity.MicroService.Data.DatabaseContext;
using Identity.MicroService.Features.UserFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Commands
{
    public class IfUserHasRoleHandler : IRequestHandler<GetUsernameRoleRequest, bool>
    {
        private readonly UserContext _userContext;

        public IfUserHasRoleHandler(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<bool> Handle(GetUsernameRoleRequest request, CancellationToken cancellationToken)
        {
            var item = await _userContext.Users.Where(x => x.UserName == request.UserName && x.UserRoleId == request.RoleId).AnyAsync();
            return item;
        }
    }
}
