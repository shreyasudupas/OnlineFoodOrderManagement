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
    public class DeleteCartItemInformationTests
    {
        DeleteUserCartItemCommandHandler handler;
        Mock<IRedisCacheBasketService> mockRedisCacheService;
        Mock<ILogger<DeleteUserCartItemCommandHandler>> mockLog;
        UserCartInformation userMockResult;
        public DeleteCartItemInformationTests()
        {
            mockRedisCacheService = new Mock<IRedisCacheBasketService>();
            mockLog = new Mock<ILogger<DeleteUserCartItemCommandHandler>>();

            handler = new DeleteUserCartItemCommandHandler(mockRedisCacheService.Object,
                mockLog.Object);

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

            mockRedisCacheService.Setup(_ => _.GetBasketItems("user")).ReturnsAsync(userMockResult);
            mockRedisCacheService.Setup(_ => _.UpdateBasketItems("user", It.IsAny<UserCartInformation>()))
                .ReturnsAsync(true);
        }

        [Fact]
        public async Task DeleteUserCartItemCommandHandler_DeleteAllItem_Success()
        {
            var request = new DeleteUserCartItemCommand
            {
                Username = "user"
            };
            var actual = await handler.Handle(request,It.IsAny<CancellationToken>());

            actual.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserCartItemCommandHandler_DeleteAllItem_Failure_InvalidUsername()
        {
            var request = new DeleteUserCartItemCommand
            {
                Username = "test"
            };
            var actual = await handler.Handle(request, It.IsAny<CancellationToken>());

            actual.Should().BeFalse();
        }
    }
}
