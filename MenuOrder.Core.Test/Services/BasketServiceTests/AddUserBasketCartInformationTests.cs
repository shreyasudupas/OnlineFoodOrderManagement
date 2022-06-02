using FluentAssertions;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.BasketService;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Command;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuManagement.Core.Test.Services.BasketServiceTests
{
    public class AddUserBasketCartInformationTests
    {
        public AddUserBasketCartInformationTests()
        {

        }

        [Fact]
        public async Task AddUserBasketCartInformation_AddMenuItem_In_Basket_For_The_FirstTime()
        {
            var mockLog = new Mock<ILogger<AddUserBasketCartInformationCommandHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            //No items present
            var basketInfo = new UserCartInformation
            {
                UserInfo = new UserInfoCart
                {
                    UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Username = "test"
                }
            };


            mockCacheService.Setup(_ => _.GetBasketItems(It.IsAny<string>()))
                .Returns(Task.FromResult(basketInfo));

            mockCacheService.Setup(_ => _.UpdateBasketItems(It.IsAny<string>(),It.IsAny<UserCartInformation>()))
                .Returns(Task.FromResult(true));

            var handler = new AddUserBasketCartInformationCommandHandler(mockCacheService.Object,mockLog.Object);

            var cartInfo = JObject.Parse("{\"ColumnData\":[{\"column name\":\"id\",\"property type\":\"ID\",\"column description\":\"This is Primary key for the header\",\"display name\":\"ID\",\"display screen\":\"none\"},{\"column name\":\"menu name\",\"propertytype\":\"string\",\"column description\":\"This is name of the menu\",\"display name\":\"Menu\",\"display screen\":\"\"},{\"column name\":\"menu type\",\"property type\":\"string\",\"column description\":\"This is type of menu\",\"display name\":\"Menu Type\",\"display screen\":\"\"},{\"column name\":\"Price\",\"property type\":\"number\",\"column description\":\"This is cost of the Item\",\"display name\":\"Price\",\"display screen\":\"\"}],\"Data\":{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"idly\",\"menu type\":\"breakfast\",\"Price\":20,\"quantity\":2},\"vendor details\":{\"id\":\"ab5ee2b1-b64f-40a2-8171-61e5b479b940\",\"name\":\"Sukh Sagar\"}}");

            var actual = await handler.Handle(new AddUserBasketCartInformationCommand 
            {
                Username = "test",
                CartInformation = cartInfo
            },It.IsAny<CancellationToken>());

            actual.Should().BeTrue();

        }

        [Fact]
        public async Task AddUserBasketCartInformation_AddMenuItem_In_Basket_ShouldThrowError_CartInfo_MenuDataObject_Is_InCorrect()
        {
            var mockLog = new Mock<ILogger<AddUserBasketCartInformationCommandHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            //No items present
            var basketInfo = new UserCartInformation
            {
                UserInfo = new UserInfoCart
                {
                    UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Username = "test",
                }
            };


            mockCacheService.Setup(_ => _.GetBasketItems(It.IsAny<string>()))
                .Returns(Task.FromResult(basketInfo));

            mockCacheService.Setup(_ => _.UpdateBasketItems(It.IsAny<string>(), It.IsAny<UserCartInformation>()))
                .Returns(Task.FromResult(true));

            var handler = new AddUserBasketCartInformationCommandHandler(mockCacheService.Object, mockLog.Object);

            var cartInfo = JObject.Parse("{\"ColumnData\":[{\"column name\":\"id\",\"property type\":\"ID\",\"column description\":\"This is Primary key for the header\",\"display name\":\"ID\",\"display screen\":\"none\"},{\"column name\":\"menu name\",\"propertytype\":\"string\",\"column description\":\"This is name of the menu\",\"display name\":\"Menu\",\"display screen\":\"\"},{\"column name\":\"menu type\",\"property type\":\"string\",\"column description\":\"This is type of menu\",\"display name\":\"Menu Type\",\"display screen\":\"\"},{\"column name\":\"Price\",\"property type\":\"number\",\"column description\":\"This is cost of the Item\",\"display name\":\"Price\",\"display screen\":\"\"}],\"Data\":[{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"idly\",\"menu type\":\"breakfast\",\"Price\":20,\"quantity\":2}],\"vendor details\":{\"id\":\"ab5ee2b1-b64f-40a2-8171-61e5b479b940\",\"name\":\"Sukh Sagar\"}}");

            await FluentActions.Invoking(()=> handler.Handle(new AddUserBasketCartInformationCommand
            {
                Username = "test",
                CartInformation = cartInfo
            }, It.IsAny<CancellationToken>())).Should().ThrowAsync<ArgumentException>();
        }
    }
}
