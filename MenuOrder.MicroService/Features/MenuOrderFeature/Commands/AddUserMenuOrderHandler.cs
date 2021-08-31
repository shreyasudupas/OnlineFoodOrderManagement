using MediatR;
using MenuOrder.MicroService.Data;
using MenuOrder.MicroService.Features.MenuOrderFeature.Querries;
using MenuOrder.MicroService.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MicroService.Shared.Models;
using MenuOrder.MicroService.Data.Enum;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Commands
{
    public class AddUserMenuOrderHandler : IRequestHandler<AddUserMenuOrder, MenuOrderResponse>
    {
        private readonly OrderRepository orderRepository;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public AddUserMenuOrderHandler(OrderRepository orderRepository, IConnectionMultiplexer connectionMultiplexer)
        {
            this.orderRepository = orderRepository;
            _connectionMultiplexer = connectionMultiplexer;
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
                        NewUser.UserInfo = new Data.UserInfo
                        {
                            Id = BasketUserInfo.Id,
                            UserName = BasketUserInfo.UserName,
                            RoleName = BasketUserInfo.RoleName,
                            FullName = BasketUserInfo.Nickname,
                            Address = BasketUserInfo.Address,
                            CityName = BasketUserInfo.CityName,
                            StateName = BasketUserInfo.StateName,
                            PictureLocation = BasketUserInfo.PictureLocation
                        };
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
