using AutoMapper;
using IdenitityServer.Core.Common.Mapping;
using IdenitityServer.Core.Domain.Response;

namespace IdenitityServer.Core.Mediators.Login
{
    public class LoginCommandDto : IMapFrom<LoginResponse>
    {
        public bool RedirectRequired { get; set; }
        public string Error { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginResponse, LoginCommandDto>()
                .ReverseMap();
        }
    }
}
