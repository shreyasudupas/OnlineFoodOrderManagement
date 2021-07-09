using AutoMapper;
using Identity.MicroService.Data.DatabaseContext;
using Identity.MicroService.Features.UserFeature.Queries;
using MediatR;
using MicroService.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Commands
{
    public class GetUpdateUserHandler : IRequestHandler<GetUserRequestModel, Users>
    {
        private readonly UserContext _userContext;
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public GetUpdateUserHandler(UserContext userContext, IMapper mapper, IConnectionMultiplexer connectionMultiplexer)
        {
            _userContext = userContext;
            _mapper = mapper;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<Users> Handle(GetUserRequestModel request, CancellationToken cancellationToken)
        {
            Users UserProfile = new Users();
            var db = _connectionMultiplexer.GetDatabase();
            var getUserInfoFromCache = await db.StringGetAsync(request.Username);

            if(!getUserInfoFromCache.HasValue)
            {
                var user = await _userContext.Users
                    .Include(x=>x.State)
                    .Include(x=>x.City).Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
                if (user != null)
                {
                    UserProfile = _mapper.Map<Users>(user);

                    UserCartInfo info = new UserCartInfo();
                    info.UserInfo = _mapper.Map<UserInfo>(UserProfile);

                    await db.StringSetAsync(request.Username, JsonConvert.SerializeObject(info));
                }
            }
            else
            {
                var user = await _userContext.Users
                    .Include(x => x.State)
                    .Include(x => x.City).Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
                if (user != null)
                {
                    UserProfile = _mapper.Map<Users>(user);
                }
            }
            
            //throw new Exception("I am throwing exceptions");
            
            return UserProfile;
        }
    }
}
