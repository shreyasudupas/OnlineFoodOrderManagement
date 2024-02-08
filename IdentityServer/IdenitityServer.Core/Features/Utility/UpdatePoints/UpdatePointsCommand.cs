using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility.UpdatePoints
{
    public class UpdatePointsCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public double AmountToBeReversed { get; set; }
    }

    public class UpdatePointsCommandHandler : IRequestHandler<UpdatePointsCommand, bool>
    {
        private readonly IUserService _userService;

        public UpdatePointsCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(UpdatePointsCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateReversePoints(request.UserId,request.AmountToBeReversed);
        }
    }
}
