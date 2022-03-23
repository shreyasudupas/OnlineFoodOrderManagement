using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;

namespace MenuManagement_IdentityServer.AutoMapperProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserInformationModel>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, act => act.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                .ForMember(dest => dest.Points, act => act.MapFrom(src => src.Points))
                .ForMember(dest => dest.CartAmount, act => act.MapFrom(src => src.CartAmount))
                .ForMember(dest => dest.ImagePath, act => act.MapFrom(src => src.ImagePath));

            CreateMap<UserAddress, UserAdressModel>()
                .ForMember(dest => dest.FullAddress, act => act.MapFrom(src => src.FullAddress))
                .ForMember(dest => dest.City, act => act.MapFrom(src => src.City))
                .ForMember(dest => dest.State, act => act.MapFrom(src => src.State))
                .ForMember(dest => dest.IsActive, act => act.MapFrom(src => src.IsActive));
        }
    }
}
