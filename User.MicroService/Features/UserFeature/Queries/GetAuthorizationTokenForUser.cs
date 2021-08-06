using Identity.MicroService.Models.Authentication;
using MediatR;

namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class GetAuthorizationTokenForUser:IRequest<AuthenticationResponse>
    {
        public string  UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
