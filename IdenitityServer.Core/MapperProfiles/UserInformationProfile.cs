using AutoMapper;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Types.OutputTypes;

namespace IdenitityServer.Core.MapperProfiles
{
    public class UserInformationProfile :Profile
    {
        public UserInformationProfile()
        {
            CreateMap<UserProfile, UserInformationOutputType>()
                .ReverseMap();
        }
    }
}
