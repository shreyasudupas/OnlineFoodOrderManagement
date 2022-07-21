using AutoMapper;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Types.OutputTypes;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class GetUserInformationResolver
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserInformationResolver(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserInformationOutputType> GetUserInfo(string UserId)
        {
            var result = await _userService.GetUserInformationById(UserId);

            var modelMapToOutputType = _mapper.Map<UserInformationOutputType>(result);

            return modelMapToOutputType;
        }
    }
}
