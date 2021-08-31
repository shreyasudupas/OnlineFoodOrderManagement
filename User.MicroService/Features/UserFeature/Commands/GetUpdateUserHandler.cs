using AutoMapper;
using Identity.MicroService.Data.DatabaseContext;
using Identity.MicroService.Features.UserFeature.Queries;
using MediatR;
using Common.Utility.Models;
using Common.Utility.Models.CartInformationModels;
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

            //If no  value present in cache then add it
            if (!getUserInfoFromCache.HasValue)
            {
                var GetUserProfile = await _userContext.Users
                    .Include(x => x.State)
                    .Include(x => x.City).Where(x => x.UserName == request.Username).FirstOrDefaultAsync();

                if (GetUserProfile != null)
                {
                    UserCartInformation UserCartInfo = new UserCartInformation();
                    UserCartInfo.UserInfo = _mapper.Map<UserInfo>(GetUserProfile);
                    UserCartInfo.VendorDetails = null;
                    UserCartInfo.Items = null;

                    await db.StringSetAsync(request.Username, JsonConvert.SerializeObject(UserCartInfo));
                }
                return UserProfile;
            }
            else
            {
                //if present then display from cache
                var UserInfoInCache = JsonConvert.DeserializeObject<UserCartInformation>(getUserInfoFromCache);
                var UserProfileFromDB_Converted = _mapper.Map<Users>(UserInfoInCache.UserInfo);
                return UserProfileFromDB_Converted;
            }
            //throw new Exception("I am throwing exceptions");
        }
    }
}
