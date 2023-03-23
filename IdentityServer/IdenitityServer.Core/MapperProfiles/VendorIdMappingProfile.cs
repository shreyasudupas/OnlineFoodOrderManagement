using AutoMapper;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;

namespace IdenitityServer.Core.MapperProfiles
{
    public class VendorIdMappingProfile : Profile
    {
        public VendorIdMappingProfile()
        {
            CreateMap<VendorIdMappingResponse, VendorUserIdMapping>()
                .ReverseMap();
        }
    }
}
