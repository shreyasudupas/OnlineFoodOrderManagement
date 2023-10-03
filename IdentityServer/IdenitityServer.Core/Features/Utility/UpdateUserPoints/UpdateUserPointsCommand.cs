using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility.UpdateUserPoints
{
    public class UpdateUserPointsCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public double AmountToBeDebited { get; set; }
    }

    public class UpdateUserPointsCommandHandler : IRequestHandler<UpdateUserPointsCommand, bool>
    {
        private readonly IUserService _userService;

        public UpdateUserPointsCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(UpdateUserPointsCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateUserPoints(request.UserId, request.AmountToBeDebited);

            return result;
        }
    }
}
