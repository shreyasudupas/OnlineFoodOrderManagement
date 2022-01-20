using AutoMapper;
using MenuManagement_IdentityServer.Controllers.Administration;
using MenuManagement_IdentityServer.Models;

namespace MenuManagement_IdentityServer.AutoMapperProfile
{
    public class EditRoleProfile : Profile
    {
        public EditRoleProfile()
        {
            CreateMap<EditRoleGet, EditRoleViewModel>()
                .ForMember(dest => dest.Id, source => source.MapFrom(s => s.Id))
                .ForMember(dest => dest.RoleName, source => source.MapFrom(s => s.RoleName))
                .ForMember(dest => dest.UserNames, source => source.MapFrom(s => s.Username));
        }
    }
}
