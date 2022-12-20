using AutoMapper;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.Login;

namespace IdenitityServer.Core.MapperProfiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginResponse, LoginCommandDto>()
                .ForMember(dest => dest.isSuccess, act => act.MapFrom(src => string.IsNullOrEmpty(src.Error) ? true : false))
                .ReverseMap();
        }
    }
}
