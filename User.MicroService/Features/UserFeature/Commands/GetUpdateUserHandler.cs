using AutoMapper;
using Identity.MicroService.Data.DatabaseContext;
using Identity.MicroService.Features.UserFeature.Queries;
using MediatR;
using MicroService.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Commands
{
    public class GetUpdateUserHandler : IRequestHandler<AddUserRequestModel, Users>
    {
        private readonly UserContext _userContext;
        private readonly IMapper _mapper;

        public GetUpdateUserHandler(UserContext userContext, IMapper mapper)
        {
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<Users> Handle(AddUserRequestModel request, CancellationToken cancellationToken)
        {
            Users UserProfile = new Users();

            var user = await _userContext.Users.Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
            if (user == null)
            {
                user.UserName = request.Username;
                user.FullName = request.NickName;
                user.PictureLocation = request.PictureLocation;
                user.RoleId = request.RoleId;
                user.Points = 0;
                user.CartAmount = 0;
                user.CreatedDate = DateTime.Now;

                _userContext.Users.Add(user);
                _userContext.SaveChanges();
            }
            //throw new Exception("I am throwing exceptions");

            UserProfile = _mapper.Map<Users>(user);

            return UserProfile;
        }
    }
}
