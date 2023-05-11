using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;

namespace MenuManagement_IdentityServer.AutoMapperProfile
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<ApplicationUser, UserInfomationModel>()
                .ForMember(dest=>dest.Address,act=>act.MapFrom(src=>src.Address))
                .ForMember(dest=>dest.ImagePath,src=>src.MapFrom(s=>s.ImagePath));
                
        }
    }
}
