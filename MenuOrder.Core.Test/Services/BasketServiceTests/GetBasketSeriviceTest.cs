using FluentAssertions;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.BasketService;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Query;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuOrder.Core.Test.Services.BasketServiceTests
{
    public class GetBasketSeriviceTest
    {
        public GetBasketSeriviceTest()
        {

        }

        public UserCartInformation GetUserCartInfo()
        {
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

            var actual = await handler.Handle(new GetBasketInformationQuery(), It.IsAny<CancellationToken>());

            var expected = "{\"UserInfo\":{\"UserId\":\"e56ad6b2-210a-4ac0-b7c2-418b4eda9eed\",\"Username\":\"test\",\"Email\":\"test@test.com\",\"PhoneNumber\":\"122121\",\"Address\":[{\"FullAddress\":\"sample\",\"City\":\"bangalore\",\"State\":\"Karnataka\",\"IsActive\":true}],\"ImagePath\":\"test.png\",\"CartAmount\":10,\"Points\":100.0},\"Items\":null,\"VendorDetails\":null}";
            actual.Should().NotBeNullOrEmpty();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
