using AutoMapper;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using System;

namespace MenuManagment.Mongo.Domain.MappingProfiles.OrderManagement
{
    public class OrderInformationProfile : Profile
    {
        public OrderInformationProfile()
        {
            CreateMap<OrderInformationDto, OrderInformation>()
                .ForMember(dest => dest.OrderPlaced, act => act.MapFrom((src, dest) =>
                {
                    if (string.IsNullOrEmpty(src.OrderPlaced))
                        return new DateTime();
                    else
                    {
                        var date = DateTime.Parse(src.OrderPlaced);
                        return date;
                    }
                }));

            CreateMap<OrderInformation,OrderInformationDto>()
                .ForMember(dest => dest.OrderPlaced, act => act.MapFrom((src, dest) =>
                {
                    return src.OrderPlaced.ToLocalTime();
                }));

            CreateMap<PaymentOrderDetailDto, PaymentOrderDetail>()
                .ReverseMap();

            CreateMap<UserOrderDetailsDto,UserOrderDetails>()
                .ReverseMap();
        }
    }
}
