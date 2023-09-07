using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Register
{
    public class RegisterQuery : IRequest<RegisterResponse>
    {
        public string ReturnUrl { get; set; }
    }

    public class RegisterQueryHandler : IRequestHandler<RegisterQuery, RegisterResponse>
    {
        private readonly IUtilsService _utilsService;

        public RegisterQueryHandler(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }
        public Task<RegisterResponse> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
            var response = new RegisterResponse
            {
                ReturnUrl = request.ReturnUrl
            };

            return Task.FromResult(response);
        }
    }
}
