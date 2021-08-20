using AutoMapper;
using Identity.MicroService.Data;
using MicroService.Shared.Models;
using System.Collections.Generic;

namespace Identity.MicroService.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, Users>()
                .ForMember(dest => dest.StateName, source => source.MapFrom(s => s.StateId != null ? s.State.StateNames : string.Empty))
                .ForMember(dest => dest.CityName, source => source.MapFrom(s => s.CityId != null ? s.City.CityNames : string.Empty))
                .ForMember(dest => dest.CreatedDate, source => source.MapFrom(s => s.CreatedDate.ToString("dddd, dd MMMM yyyy")))
                .ForMember(dest => dest.UpdatedDate, source => source.MapFrom(s => s.UpdatedDate.GetValueOrDefault().ToString("dddd, dd MMMM yyyy")));

            CreateMap<Users,UserInfo>()
                .ForMember(dest => dest.StateName, source => source.MapFrom(s => s.StateName))
                .ForMember(dest => dest.CityName, source => source.MapFrom(s => s.CityName))
                .ForMember(dest => dest.RoleName, source => source.MapFrom(s => s.RoleId == 1?"User":"Admin"));

            CreateMap<UserInfo, Users>()
                .ForMember(dest => dest.RoleId, act => act.MapFrom(src => (src.RoleName.Contains("User")) ? 1 : 2));
        }
    }
}
