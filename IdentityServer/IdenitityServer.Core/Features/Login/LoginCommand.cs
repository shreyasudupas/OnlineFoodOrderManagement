using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Login
{
    public class LoginCommand : IRequest<LoginCommandDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public bool isSuccess { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandDto>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(IAuthService authService,
            IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _authService.Login(request);

            var resultMapToDto = _mapper.Map<LoginCommandDto>(loginResult);
            return resultMapToDto;
        }
    }
}
