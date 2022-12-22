using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Register
{
    public class RegisterAdminQuery : IRequest<RegisterAdminResponse>
    {
        public RegisterAdminResponse registerAdminResponse { get; set; }
    }

    public class RegisterAdminResponseHandler : IRequestHandler<RegisterAdminQuery, RegisterAdminResponse>
    {
        private readonly IAuthService authService;

        public RegisterAdminResponseHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<RegisterAdminResponse> Handle(RegisterAdminQuery request, CancellationToken cancellationToken)
        {
            return await authService.RegisterAdmin(request.registerAdminResponse);
        }
    }
}
