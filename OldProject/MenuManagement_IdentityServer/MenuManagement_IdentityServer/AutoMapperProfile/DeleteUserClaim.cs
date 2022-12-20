using AutoMapper;
using MenuManagement_IdentityServer.Models;
using System.Security.Claims;

namespace MenuManagement_IdentityServer.AutoMapperProfile
{
    public class DeleteUserClaim : Profile
    {
        public DeleteUserClaim()
        {
            CreateMap<Claim, UserClaimList>()
                .ForMember(dest => dest.ClaimType, source => source.MapFrom(s => s.Type))
                .ForMember(dest => dest.ClaimValue, source => source.MapFrom(s => s.Value));

            CreateMap<UserClaimList, DeleteUserClaimViewModel>();
        }
    }
}
