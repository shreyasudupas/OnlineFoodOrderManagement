using AutoMapper;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Enum;
using System;

namespace MenuManagment.Mongo.Domain.MappingProfiles.OrderManagement
{
    public class OrderInformationProfile : Profile
    {
        public OrderInformationProfile()
        {
            CreateMap<OrderInformationDto, OrderInformation>()
                .ForMember(dest=> dest.OrderStatus,act=>act.MapFrom((src,dest)=>
                {
                    return src.OrderStatus.ToString();
                }))
                .ForMember(dest => dest.OrderPlacedDateTime, act => act.MapFrom((src, dest) =>
                {
                    if (string.IsNullOrEmpty(src.OrderPlacedDateTime))
                        return new DateTime();
                    else
                    {
                        var date = DateTime.Parse(src.OrderPlacedDateTime);
                        return date;
                    }
                }));

            CreateMap<OrderInformation,OrderInformationDto>()
                .ForMember(dest => dest.OrderStatus, act => act.MapFrom((src, dest) =>
                {
                    OrderStatusEnum orderStatusEnum;
                    var enumConverterEval = System.Enum.TryParse(src.OrderStatus, out orderStatusEnum);

                    return orderStatusEnum;
                }))
                .ForMember(dest => dest.OrderPlacedDateTime, act => act.MapFrom((src, dest) =>
                {
                    return src.OrderPlacedDateTime.ToLocalTime();
                }));

            CreateMap<MenuItemDto,MenuItem>()
                .ReverseMap();


            CreateMap<PaymentOrderDetailDto, PaymentOrderDetail>()
                .ForMember(dest=>dest.Price,act=>act.MapFrom(src=>src.TotalPrice))
                .ForMember(dest => dest.PaymentSuccess, act => act.MapFrom(src => src.PaymentSuccess))
                .ForMember(dest => dest.MethodOfDelivery, act => act.MapFrom(src => src.MethodOfDelivery))
                .ForMember(dest => dest.SelectedPayment, act => act.MapFrom(src => src.SelectedPayment))
                .ReverseMap();

            CreateMap<UserOrderDetailsDto, UserOrderDetails>()
                .ReverseMap();
        }
    }
}
