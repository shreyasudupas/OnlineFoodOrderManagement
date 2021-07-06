using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class GetUsernameRoleRequest:IRequest<bool>
    {
        public string UserName { get; set; }
        public long RoleId { get; set; }
    }
}
