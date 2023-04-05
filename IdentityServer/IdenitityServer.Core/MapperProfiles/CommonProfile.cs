using AutoMapper;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;

namespace IdenitityServer.Core.MapperProfiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<VendorUserIdMapping, VendorMappingResponse>()
                .ReverseMap();
        }
    }
}
