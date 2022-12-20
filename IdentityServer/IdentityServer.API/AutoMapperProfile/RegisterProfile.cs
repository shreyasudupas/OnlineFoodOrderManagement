using AutoMapper;
using IdenitityServer.Core.Domain.Response;
using IdenitityServer.Core.Features.Register;
using IdentityServer.API.Controllers.ViewModels;

namespace IdentityServer.API.AutoMapperProfile
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterResponse, RegisterViewModel>()
                .ReverseMap();
            CreateMap<RegisterCommand, RegisterViewModel>()
                .ReverseMap();
        }
    }
}
