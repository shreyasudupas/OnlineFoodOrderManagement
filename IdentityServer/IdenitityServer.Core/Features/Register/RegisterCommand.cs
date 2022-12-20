using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Register
{
    public class RegisterCommand: RegisterResponse, IRequest
    {
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        async Task<Unit> IRequestHandler<RegisterCommand, Unit>.Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _authService.Register(request);
            return Unit.Value;
        }
    }
}
