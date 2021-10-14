using MediatR;
using Common.Utility.Models;

namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class GetUserRequestModel:IRequest<UserInfo>
   {
        public string Username { get; set; }
    }
}
