using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Logout
{
    public class LogoutQuery : IRequest<PreLogoutResponse>
    {
        public string LogoutId { get; set; }
        public bool IsAuthenticated { get; set; }
    }

    public class LogoutQueryHandler : IRequestHandler<LogoutQuery, PreLogoutResponse>
    {
        private readonly IAuthService _authService;

        public LogoutQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<PreLogoutResponse> Handle(LogoutQuery request, CancellationToken cancellationToken)
        {
            return _authService.PreLogout(request);
        }
    }
}
