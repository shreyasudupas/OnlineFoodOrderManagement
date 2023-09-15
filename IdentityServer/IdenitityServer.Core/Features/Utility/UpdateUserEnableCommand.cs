using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class UpdateUserEnableCommand : IRequest<bool>
    {
        public string VendorId { get; set; }
        public bool Enable { get; set; }
    }

    public class UpdateUserEnableCommandHandler : IRequestHandler<UpdateUserEnableCommand, bool>
    {
        private readonly IUtilsService _utilsService;
        private readonly IUserService _userService;
        public UpdateUserEnableCommandHandler(IUtilsService utilsService,
            IUserService userService)
        {
            _utilsService = utilsService;
            _userService = userService;
        }

        public async Task<bool> Handle(UpdateUserEnableCommand request, CancellationToken cancellationToken)
        {
            var getUserId = await _userService.GetUserIdByVendorClaim(request.VendorId);

            if(!string.IsNullOrEmpty(getUserId))
            {
                var result = await _utilsService.UpdateUserEnable(getUserId, request.Enable);
                return result;
            }
            return false;
        }
    }
}
