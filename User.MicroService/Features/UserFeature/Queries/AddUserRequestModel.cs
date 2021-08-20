using MediatR;
using MicroService.Shared.Models;

namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class GetUserRequestModel:IRequest<Users>
   {
        public string Username { get; set; }
    }
}
