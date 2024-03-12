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
                .ForMember(dest=>dest.OrderStatusDetails,act=>act.MapFrom(src=>src.Status))
                ;

            CreateMap<OrderInformation,OrderInformationDto>()
                .ForMember(dest => dest.Status, act => act.MapFrom(src => src.OrderStatusDetails))
                ;

            CreateMap<MenuItemDto,MenuItem>()
                .ReverseMap();


            CreateMap<PaymentOrderDetailDto, PaymentOrderDetail>()
                .ForMember(dest=>dest.Price,act=>act.MapFrom(src=>src.Price))
                .ForMember(dest => dest.PaymentSuccess, act => act.MapFrom(src => src.PaymentSuccess))
                .ForMember(dest => dest.MethodOfDelivery, act => act.MapFrom(src => src.MethodOfDelivery))
                .ForMember(dest => dest.SelectedPayment, act => act.MapFrom(src => src.SelectedPayment))
                .ReverseMap();

            CreateMap<UserOrderDetailsDto, UserOrderDetail>()
                .ReverseMap();

            CreateMap<VendorDetailDto, VendorDetail>()
                .ReverseMap();

            CreateMap<OrderStatusDetailDto, OrderStatusDetail>()
                .ForMember(dest => dest.OrderPlaced, act => act.MapFrom((src, dest) =>
                {
                    if(src.OrderPlaced == null)
                    {
                        DateTime? d = null;
                        return d;
                    } 
                    else
                    {
                        var d = DateTime.Parse(src.OrderPlaced);
                        return d;
                    }
                }))
                .ForMember(dest => dest.OrderInProgress, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderInProgress == null)
                    {
                        DateTime? d = null;
                        return d;
                    }
                    else
                    {
                        var d = DateTime.Parse(src.OrderInProgress);
                        return d;
                    }
                }))
                .ForMember(dest => dest.OrderReady, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderInProgress == null)
                    {
                        DateTime? d = null;
                        return d;
                    }
                    else
                    {
                        var d = DateTime.Parse(src.OrderReady);
                        return d;
                    }
                }))
                .ForMember(dest => dest.OrderDone, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderDone == null)
                    {
                        DateTime? d = null;
                        return d;
                    }
                    else
                    {
                        var d = DateTime.Parse(src.OrderDone);
                        return d;
                    }
                }))
                .ForMember(dest => dest.OrderCancelled, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderCancelled == null)
                    {
                        DateTime? d = null;
                        return d;
                    }
                    else
                    {
                        var d = DateTime.Parse(src.OrderCancelled);
                        return d;
                    }
                }))
                ;

            CreateMap<OrderStatusDetail, OrderStatusDetailDto>()
                .ForMember(dest => dest.OrderPlaced, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderPlaced == null)
                    {
                        return null;
                    }
                    else
                    {
                        return src.OrderPlaced.ToString();
                    }
                }))
                .ForMember(dest => dest.OrderInProgress, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderInProgress == null)
                    {
                        return null;
                    }
                    else
                    {
                        return src.OrderInProgress.ToString();
                    }
                }))
                .ForMember(dest => dest.OrderReady, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderReady == null)
                    {
                        return null;
                    }
                    else
                    {
                        return src.OrderReady.ToString();
                    }
                }))
                .ForMember(dest => dest.OrderDone, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderDone == null)
                    {
                        return null;
                    }
                    else
                    {
                        return src.OrderDone.ToString();
                    }
                }))
                .ForMember(dest => dest.OrderCancelled, act => act.MapFrom((src, dest) =>
                {
                    if (src.OrderCancelled == null)
                    {
                        return null;
                    }
                    else
                    {
                        return src.OrderCancelled.ToString();
                    }
                }))
                ;
        }
    }
}
