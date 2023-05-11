using FluentAssertions;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.BasketService;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Query;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuManagement.Core.Test.Services.BasketServiceTests
{
    public class GetUserBasketCartInformationTests
    {
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
                UserInfo = new UserInfoCart
                {
                    UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Username = "test"
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

            var handler = new GetBasketInformationQueryHandler(mockLog.Object, mockCacheService.Object);

            var actual = await handler.Handle(new GetBasketInformationQuery { Username = "test" }, It.IsAny<CancellationToken>());

            var expected = "{\"UserInfo\":{\"UserId\":\"e56ad6b2-210a-4ac0-b7c2-418b4eda9eed\",\"Username\":\"test\"},\"Items\":[{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"idly\",\"menu type\":\"breakfast\",\"price\":20,\"quantity\":1},{\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b941\",\"menu name\":\"dosa\",\"menu type\":\"breakfast\",\"price\":30,\"quantity\":2}],\"VendorDetails\":{\"id\":\"e56ad6b2-210a-4ac0-b7c2-418b4eda9eed\",\"name\":\"test name\"}}";
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
    }
}
