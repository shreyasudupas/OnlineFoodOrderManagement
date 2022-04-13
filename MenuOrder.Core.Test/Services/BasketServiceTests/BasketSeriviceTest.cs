using FluentAssertions;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.BasketService;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Command;
using MenuManagement.Core.Services.BasketService.Query;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuOrder.Core.Test.Services.BasketServiceTests
{
    public class BasketSeriviceTest
    {
        public BasketSeriviceTest()
        {

        }

        public UserCartInformation GetUserCartInfo()
        {
            JObject item1 = JObject.Parse("{'id':'fb5ee2b1-b64f-40a2-8171-61e5b479b940','menu name':'idly','menu type':'breakfast','price':20,'quantity':1}");
            JObject item2 = JObject.Parse("{'id':'fb5ee2b1-b64f-40a2-8171-61e5b479b941','menu name':'dosa','menu type':'breakfast','price':30,'quantity':2}");
            var itemList = new List<JObject>
            {
                item1,item2
            };

            return new UserCartInformation
            {
                UserInfo = new UserInformationModel
                {
                    UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Username = "test",
                    CartAmount = 10,
                    Email = "test@test.com",
                    ImagePath = "test.png",
                    Points = 100,
                    PhoneNumber = "122121",
                    Address = new List<UserAdressModel>
                    {
                        new UserAdressModel { FullAddress = "sample", City = "bangalore", State = "Karnataka", IsActive = true }
                    }
                },
                Items = itemList,
                VendorDetails = new VendorDetail
                {
                    Id = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Name = "test name"
                }
            };
        }

        [Fact]
        public async Task GetBasketInformation_Success()
        {
            var mockLog = new Mock<ILogger<GetBasketInformationQueryHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            mockCacheService.Setup(_ => _.GetBasketItems(It.IsAny<string>()))
                .Returns(Task.FromResult(GetUserCartInfo()));

            var handler = new GetBasketInformationQueryHandler(mockLog.Object,mockCacheService.Object);

            var actual = await handler.Handle(new GetBasketInformationQuery { Username = "test" }, It.IsAny<CancellationToken>());

            var expected = "{\"UserInfo\":{\"UserId\":\"e56ad6b2-210a-4ac0-b7c2-418b4eda9eed\",\"Username\":\"test\",\"Email\":\"test@test.com\",\"PhoneNumber\":\"122121\",\"Address\":[{\"FullAddress\":\"sample\",\"City\":\"bangalore\",\"State\":\"Karnataka\",\"IsActive\":true}],\"ImagePath\":\"test.png\",\"CartAmount\":10,\"Points\":100.0},\"Items\":[{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"idly\",\"menu type\":\"breakfast\",\"price\":20,\"quantity\":1},{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b941\",\"menu name\":\"dosa\",\"menu type\":\"breakfast\",\"price\":30,\"quantity\":2}],\"VendorDetails\":{\"id\":\"e56ad6b2-210a-4ac0-b7c2-418b4eda9eed\",\"name\":\"test name\"}}";
            actual.Should().NotBeNullOrEmpty();
            actual.Should().BeEquivalentTo(expected);
            //Assert.Equal(expected,actual); //use this to find exact json errors
        }

        [Fact]
        public async Task GetUserBasketItemCount_MustHave3Items()
        {
            var mockLog = new Mock<ILogger<GetUserBasketItemCountQueryHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            mockCacheService.Setup(_ => _.GetBasketItems(It.IsAny<string>()))
                .Returns(Task.FromResult(GetUserCartInfo()));

            var handler = new GetUserBasketItemCountQueryHandler(mockLog.Object, mockCacheService.Object);

            var actual = await handler.Handle(new GetUserBasketItemCountQuery { Username = "test" }, It.IsAny<CancellationToken>());

            actual.Should().Be(3);
        }

        [Fact]
        public async Task ManageUserBasket_AddMenuItem_In_Basket_For_The_FirstTime()
        {
            var mockLog = new Mock<ILogger<ManageUserBasketItemCommandHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            //No items present
            var basketInfo = new UserCartInformation
            {
                UserInfo = new UserInformationModel
                {
                    UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Username = "test",
                    CartAmount = 10,
                    Email = "test@test.com",
                    ImagePath = "test.png",
                    Points = 100,
                    PhoneNumber = "122121",
                    Address = new List<UserAdressModel>
                    {
                        new UserAdressModel { FullAddress = "sample", City = "bangalore", State = "Karnataka", IsActive = true }
                    }
                }
            };


            mockCacheService.Setup(_ => _.GetBasketItems(It.IsAny<string>()))
                .Returns(Task.FromResult(basketInfo));

            mockCacheService.Setup(_ => _.UpdateBasketItems(It.IsAny<string>(),It.IsAny<UserCartInformation>()))
                .Returns(Task.FromResult(true));

            var handler = new ManageUserBasketItemCommandHandler(mockCacheService.Object,mockLog.Object);

            var cartInfo = JObject.Parse("{\"ColumnData\":[{\"column name\":\"id\",\"property type\":\"ID\",\"column description\":\"This is Primary key for the header\",\"display name\":\"ID\",\"display screen\":\"none\"},{\"column name\":\"menu name\",\"propertytype\":\"string\",\"column description\":\"This is name of the menu\",\"display name\":\"Menu\",\"display screen\":\"\"},{\"column name\":\"menu type\",\"property type\":\"string\",\"column description\":\"This is type of menu\",\"display name\":\"Menu Type\",\"display screen\":\"\"},{\"column name\":\"Price\",\"property type\":\"number\",\"column description\":\"This is cost of the Item\",\"display name\":\"Price\",\"display screen\":\"\"}],\"Data\":{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"idly\",\"menu type\":\"breakfast\",\"Price\":20,\"quantity\":2},\"vendor details\":{\"id\":\"ab5ee2b1-b64f-40a2-8171-61e5b479b940\",\"name\":\"Sukh Sagar\"}}");

            var actual = await handler.Handle(new ManageUserBasketItemCommand 
            {
                Username = "test",
                CartInformation = cartInfo
            },It.IsAny<CancellationToken>());

            actual.Should().BeTrue();

        }

        [Fact]
        public async Task ManageUserBasket_AddMenuItem_In_Basket_ShouldThrowError_CartInfo_MenuDataObject_Is_InCorrect()
        {
            var mockLog = new Mock<ILogger<ManageUserBasketItemCommandHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            //No items present
            var basketInfo = new UserCartInformation
            {
                UserInfo = new UserInformationModel
                {
                    UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Username = "test",
                    CartAmount = 10,
                    Email = "test@test.com",
                    ImagePath = "test.png",
                    Points = 100,
                    PhoneNumber = "122121",
                    Address = new List<UserAdressModel>
                    {
                        new UserAdressModel { FullAddress = "sample", City = "bangalore", State = "Karnataka", IsActive = true }
                    }
                }
            };


            mockCacheService.Setup(_ => _.GetBasketItems(It.IsAny<string>()))
                .Returns(Task.FromResult(basketInfo));

            mockCacheService.Setup(_ => _.UpdateBasketItems(It.IsAny<string>(), It.IsAny<UserCartInformation>()))
                .Returns(Task.FromResult(true));

            var handler = new ManageUserBasketItemCommandHandler(mockCacheService.Object, mockLog.Object);

            var cartInfo = JObject.Parse("{\"ColumnData\":[{\"column name\":\"id\",\"property type\":\"ID\",\"column description\":\"This is Primary key for the header\",\"display name\":\"ID\",\"display screen\":\"none\"},{\"column name\":\"menu name\",\"propertytype\":\"string\",\"column description\":\"This is name of the menu\",\"display name\":\"Menu\",\"display screen\":\"\"},{\"column name\":\"menu type\",\"property type\":\"string\",\"column description\":\"This is type of menu\",\"display name\":\"Menu Type\",\"display screen\":\"\"},{\"column name\":\"Price\",\"property type\":\"number\",\"column description\":\"This is cost of the Item\",\"display name\":\"Price\",\"display screen\":\"\"}],\"Data\":[{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"idly\",\"menu type\":\"breakfast\",\"Price\":20,\"quantity\":2}],\"vendor details\":{\"id\":\"ab5ee2b1-b64f-40a2-8171-61e5b479b940\",\"name\":\"Sukh Sagar\"}}");

            await FluentActions.Invoking(()=> handler.Handle(new ManageUserBasketItemCommand
            {
                Username = "test",
                CartInformation = cartInfo
            }, It.IsAny<CancellationToken>())).Should().ThrowAsync<ArgumentException>();
        }
    }
}
