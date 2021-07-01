using MediatR;
using MicroService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class AddUserRequestModel:IRequest<Users>
    {
        public string Username { get; set; }
        public string PictureLocation { get; set; }
        public string NickName { get; set; }
        public int RoleId { get; set; }
    }
}
