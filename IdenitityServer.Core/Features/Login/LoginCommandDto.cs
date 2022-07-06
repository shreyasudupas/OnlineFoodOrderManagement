using AutoMapper;
using IdenitityServer.Core.Common.Mapping;
using IdenitityServer.Core.Domain.Response;

namespace IdenitityServer.Core.Features.Login
{
    public class LoginCommandDto : IMapFrom<LoginResponse>
    {
        public bool RedirectRequired { get; set; }
        public string Error { get; set; }
        public bool isSuccess { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginResponse, LoginCommandDto>()
                .ForMember(dest => dest.isSuccess, act => act.MapFrom(src => string.IsNullOrEmpty(src.Error) ? true : false))
                .ReverseMap();
                
        }
    }
}
