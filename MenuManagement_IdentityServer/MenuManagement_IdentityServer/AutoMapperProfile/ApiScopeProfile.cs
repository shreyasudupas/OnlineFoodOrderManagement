using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using MenuManagement_IdentityServer.Models;

namespace MenuManagement_IdentityServer.AutoMapperProfile
{
    public class ApiScopeProfile : Profile
    {
        public ApiScopeProfile()
        {
            CreateMap<ApiScope, GetApiScopeModel>();
                
        }
    }
}
