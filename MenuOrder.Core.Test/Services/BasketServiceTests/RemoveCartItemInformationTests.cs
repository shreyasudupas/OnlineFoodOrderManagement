using FluentAssertions;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.BasketService;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Command;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuManagement.Core.Test.Services.BasketServiceTests
{
    public class RemoveCartItemInformationTests
    {
        RemoveUserCartItemCommandHandler handler;
        Mock<ILogger<RemoveUserCartItemCommandHandler>> mockLog;
        Mock<IRedisCacheBasketService> mockRedisBasketService;
        UserCartInformation userMockResult;

        public RemoveCartItemInformationTests()
        {
            mockLog = new Mock<ILogger<RemoveUserCartItemCommandHandler>>();
            mockRedisBasketService = new Mock<IRedisCacheBasketService>();

            handler = new RemoveUserCartItemCommandHandler(mockLog.Object, mockRedisBasketService.Object);

            JObject item1 = JObject.Parse("{'id':'fb5ee2b1-b64f-40a2-8171-61e5b479b940','menu name':'idly','menu type':'breakfast','price':20,'quantity':1}");
            JObject item2 = JObject.Parse("{'id':'fb5ee2b1-b64f-40a2-8171-61e5b479b941','menu name':'dosa','menu type':'breakfast','price':30,'quantity':2}");
            var itemList = new List<JObject>
            {
                item1,item2
            };
            userMockResult = new UserCartInformation
            {
                UserInfo = new UserInfoCart
                {
                    UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Username = "user"
                },
                Items = itemList,
                VendorDetails = new VendorDetail
                {
                    Id = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                    Name = "Sukh Sagar"
                }
            };
            mockRedisBasketService.Setup(_ => _.GetBasketItems("user"))
                .ReturnsAsync(userMockResult);
            mockRedisBasketService.Setup(_ => _.UpdateBasketItems("user", It.IsAny<UserCartInformation>()))
                .ReturnsAsync(true);
            
        }

        [Fact]
        public async Task RemoveUserCartItemCommandHandler_RemoveCartItem_RemoveIdly_MustHaveOneItem_Success()
        {
            var requestCartInfo = JObject.Parse("{\"ColumnData\":[{\"field\":\"id\",\"header\":\"ID\",\"display\":\"none\"},{\"field\":\"menu  name\",\"header\":\"Menu\",\"display\":\"\"},{\"field\":\"menu type\",\"header\":\"Menu Type\",\"display\":\"\"},{\"field\":\"Price\",\"header\":\"Price\",\"display\":\"\"},{\"field\":\"quantity\",\"header\":\"Quantity\",\"display\":\"\"}],\"Data\":{\"quantity\":0,\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"idly\",\"menu type\":\"breakfast\",\"Price\":20},\"vendor details\":{\"id\":\"628e32e932de9d36b1eedd8a\",\"name\":\"A2B\"}}");
            var actual = await handler.Handle(new RemoveUserCartItemCommand { Username = "user" , CartInformation = requestCartInfo }, It.IsAny<CancellationToken>());

            actual.Should().BeTrue();
            userMockResult.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task RemoveUserCartItemCommandHandler_RemoveCartItem_RemoveDosa_HaveOneItemEach_Success()
        {
            var requestCartInfo = JObject.Parse("{\"ColumnData\":[{\"field\":\"id\",\"header\":\"ID\",\"display\":\"none\"},{\"field\":\"menu  name\",\"header\":\"Menu\",\"display\":\"\"},{\"field\":\"menu type\",\"header\":\"Menu Type\",\"display\":\"\"},{\"field\":\"Price\",\"header\":\"Price\",\"display\":\"\"},{\"field\":\"quantity\",\"header\":\"Quantity\",\"display\":\"\"}],\"Data\":{\"quantity\":1,\"id\":\"fb5ee2b1-b64f-40a2-8171-61e5b479b940\",\"menu name\":\"dosa\",\"menu type\":\"breakfast\",\"Price\":30},\"vendor details\":{\"id\":\"628e32e932de9d36b1eedd8a\",\"name\":\"A2B\"}}");
            var actual = await handler.Handle(new RemoveUserCartItemCommand { Username = "user", CartInformation = requestCartInfo }, It.IsAny<CancellationToken>());

            actual.Should().BeTrue();
            userMockResult.Items.Should().HaveCount(2);
        }
    }
}
