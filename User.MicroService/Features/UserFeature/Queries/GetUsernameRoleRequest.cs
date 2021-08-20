using MediatR;


namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class GetUsernameRoleRequest:IRequest<bool>
    {
        public string UserName { get; set; }
        public long RoleId { get; set; }
    }
}
