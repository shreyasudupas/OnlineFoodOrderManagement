using AutoMapper;
using Identity.MicroService.Data.DatabaseContext;
using Identity.MicroService.Features.UserFeature.Queries;
using MediatR;
using Common.Utility.Models;
using Common.Utility.Models.CartInformationModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Utility.Tools.RedisCache.Interface;
using System.Collections.Generic;

namespace Identity.MicroService.Features.UserFeature.Commands
{
    public class UpdateUserHandler : IRequestHandler<GetUserRequestModel, UserInfo>
    {
        private readonly UserContext _userContext;
        private readonly IMapper _mapper;
        private readonly IGetCacheBasketItemsService _getCacheBasketItemsService;
        public UpdateUserHandler(UserContext userContext, IMapper mapper, IGetCacheBasketItemsService getCacheBasketItemsService)
        {
            _userContext = userContext;
            _mapper = mapper;
            _getCacheBasketItemsService = getCacheBasketItemsService;
        }

        public async Task<UserInfo> Handle(GetUserRequestModel request, CancellationToken cancellationToken)
        {
            UserInfo UserProfileResponse = new UserInfo();
            
            //Get Item and User info from cache service
            var getUserInfoFromCache = await _getCacheBasketItemsService.GetCacheItem(request.Username);

            //If no  value present in cache then add it
            if (!getUserInfoFromCache.HasValue)
            {
                var GetUserProfile = await _userContext.Users.Include(x => x.UserRoleEntityFK)
                    .Where(x => x.UserName == request.Username).FirstOrDefaultAsync();

                if (GetUserProfile != null)
                {
                    var UserAddress = await _userContext.UserAddresses
                        .Include(x=>x.CityEntityFK).ThenInclude(x=>x.StateEntityFK)
                        .Where(ua => ua.UserId == GetUserProfile.Id ).ToListAsync();
                    
                    UserCartInformation UserCartInfo = new UserCartInformation();
                    UserCartInfo.UserInfo = _mapper.Map<UserInfo>(GetUserProfile);
                    UserCartInfo.VendorDetails = null;
                    UserCartInfo.Items = null;

                    //If Address is present or else save without address
                    if (UserAddress.Count > 0)
                    {
                        //Initilize or else will get error
                        UserCartInfo.UserInfo.Address = new List<UserAddress>();

                        UserAddress.ForEach(element=> {

                            UserCartInfo.UserInfo.Address.Add(new UserAddress
                            {
                                UserAddressId = element.Id,
                                City = element.CityEntityFK.CityNames,
                                FullAddress = element.Address,
                                State = element.CityEntityFK.StateEntityFK.StateNames,
                                IsActive = element.IsActive
                            });
                        });

                    }

                    //await db.StringSetAsync(request.Username, JsonConvert.SerializeObject(UserCartInfo));
                    await _getCacheBasketItemsService.SetCacheItem(request.Username, UserCartInfo);

                    UserProfileResponse = UserCartInfo.UserInfo;

                }
                return UserProfileResponse;
            }
            else
            {
                //if present then display from cache
                var UserInfoInCache = JsonConvert.DeserializeObject<UserCartInformation>(getUserInfoFromCache);

                UserProfileResponse = UserInfoInCache.UserInfo;

                return UserProfileResponse;
            }
            //throw new Exception("I am throwing exceptions");
        }
    }
}
