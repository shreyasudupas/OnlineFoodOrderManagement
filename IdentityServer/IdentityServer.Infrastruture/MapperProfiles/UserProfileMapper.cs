using AutoMapper;
using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;

namespace IdentityServer.Infrastruture.MapperProfiles
{
    public class UserProfileMapper : Profile
    {
        public UserProfileMapper()
        {
            CreateMap<ApplicationUser, UserProfile>();
        }
    }
}
