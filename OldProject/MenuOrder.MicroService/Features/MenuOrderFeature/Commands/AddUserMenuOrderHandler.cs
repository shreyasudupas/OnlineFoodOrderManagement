﻿using MediatR;
using MenuOrder.MicroService.Features.MenuOrderFeature.Querries;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Common.Utility.Models;
using MenuOrder.MicroService.Features.MenuOrderFeature.Response;
using Common.Mongo.Database.Data.Context;
using Common.Mongo.Database.Data.Models;
using Common.Mongo.Database.Data.Enum;
using AutoMapper;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Commands
{
    public class AddUserMenuOrderHandler : IRequestHandler<AddUserMenuOrder, MenuOrderResponse>
    {
        private readonly OrderRepository orderRepository;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IMapper _mapper;

        public AddUserMenuOrderHandler(OrderRepository orderRepository, IConnectionMultiplexer connectionMultiplexer, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            _connectionMultiplexer = connectionMultiplexer;
            _mapper = mapper;
        }

        public async Task<MenuOrderResponse> Handle(AddUserMenuOrder request, CancellationToken cancellationToken)
        {
            MenuOrderResponse response = new MenuOrderResponse();
            //get the order info from basket
            var database = _connectionMultiplexer.GetDatabase();
            var GetBasketData = await database.StringGetAsync(request.UserName);

            if(GetBasketData.HasValue)
            {
                var GetBasketUserItems = JsonConvert.DeserializeObject<UserCartInfo>(GetBasketData);
                if(GetBasketUserItems.Items != null)
                {
                    var getExsitingOrders = await orderRepository.GetUserOrdersBasedOnUserName(request.UserName);
                    if(getExsitingOrders == null)
                    {
                        //Then insert the user into the database
                        Orders NewUser = new Orders();
                        var BasketUserInfo = GetBasketUserItems.UserInfo;
                        NewUser.UserInfo = new Common.Mongo.Database.Data.Models.UserInfo
                        {
                            UserName = BasketUserInfo.UserName,
                            RoleName = BasketUserInfo.RoleName,
                            //Address = new Common.Mongo.Database.Data.Models.UserAddress
                            //{
                            //    City = BasketUserInfo.Address.City,
                            //    FullAddress = BasketUserInfo.Address.FullAddress,
                            //    State = BasketUserInfo.Address.State
                            //},
                            
                            PictureLocation = BasketUserInfo.PictureLocation
                        };

                        if(GetBasketUserItems.UserInfo.Address.Count>0)
                        {
                            GetBasketUserItems.UserInfo.Address.ForEach(e => {
                                var MappedAddress = _mapper.Map<Common.Mongo.Database.Data.Models.UserAddress>(e);
                                NewUser.UserInfo.Address.Add(MappedAddress);
                            });
                        }

                        await orderRepository.InsertUserOrders(NewUser);

                        //get the user Info once inserted
                        var GetUserFromDb = await orderRepository.GetUserOrdersBasedOnUserName(request.UserName);
                        var Items = AddOrderItem(GetBasketUserItems.Items);
                        GetUserFromDb.OrderItems = new List<OrderItem>();
                        GetUserFromDb.OrderItems.AddRange(Items);

                        //update in Database
                        await orderRepository.UpdateOrders(GetUserFromDb);

                        response.UserId = GetUserFromDb.UserInfo.Id;
                    }
                    else
                    {
                        var Items = AddOrderItem(GetBasketUserItems.Items);
                        getExsitingOrders.OrderItems.AddRange(Items);

                        //update in Database
                        await orderRepository.UpdateOrders(getExsitingOrders);

                        response.UserId = getExsitingOrders.UserInfo.Id;
                    }

                    //remove the item from Basket
                    GetBasketUserItems.Items = null;
                    await updateUserCartCache(database,request.UserName, GetBasketUserItems);
                }

            }
            return response;
        }

        private List<OrderItem> AddOrderItem(List<CartItems> Items)
        {
            List<OrderItem> UserItems = new List<OrderItem>();
            Items.ForEach(x => UserItems.Add(
            new OrderItem
            {
                Id = Guid.NewGuid().ToString(),
                MenuItem = x.menuItem,
                Price = x.price,
                VendorId = x.vendorId,
                VendorName = x.vendorName,
                MenuType = x.menuType,
                ImagePath = x.imagePath,
                OfferPrice = x.offerPrice,
                Quantity = x.quantity,
                CreatedDate = x.createdDate,
                Status = OrderStatus.Pending
            })
            );
            return UserItems;
        }
        public async Task updateUserCartCache(IDatabase db, string Username, UserCartInfo items)
        {
            //remove the current value from cache
            await db.KeyDeleteAsync(Username);
            //add the updated value
            var serilizeItem = JsonConvert.SerializeObject(items);
            await db.StringSetAsync(Username, serilizeItem);
        }
    }
}
