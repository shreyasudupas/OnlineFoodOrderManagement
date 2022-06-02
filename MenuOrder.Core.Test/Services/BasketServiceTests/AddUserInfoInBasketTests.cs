using FluentAssertions;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.Common;
using MenuManagement.Core.Services.BasketService.Command.AddUserInformationCommand;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MenuManagement.Core.Test.Services.BasketServiceTests
{
    public class AddUserInfoInBasketTests
    {
        [Fact]
        public async Task AddUserInformationInBasketCommandHandler_Must_Return_True_Success()
        {
            var mockLog = new Mock<ILogger<AddUserInformationInBasketCommandHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            mockCacheService.Setup(_ => _.AddUserInformationInBasket(It.IsAny<UserInfoCart>()))
                .Returns(Task.FromResult(true));

            var handler = new AddUserInformationInBasketCommandHandler(mockCacheService.Object, mockLog.Object);

            var actual = await handler.Handle(new AddUserInformationInBasketCommand
            {
                UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                Username = "test"
            }, It.IsAny<CancellationToken>());

            actual.Should().BeTrue();

        }

        [Fact]
        public async Task AddUserInformationInBasketCommandHandler_Must_Return_False_IfError()
        {
            var mockLog = new Mock<ILogger<AddUserInformationInBasketCommandHandler>>();
            var mockCacheService = new Mock<IRedisCacheBasketService>();

            mockCacheService.Setup(_ => _.AddUserInformationInBasket(It.IsAny<UserInfoCart>()))
                .Returns(Task.FromResult(false));

            var handler = new AddUserInformationInBasketCommandHandler(mockCacheService.Object, mockLog.Object);

            var actual = await handler.Handle(new AddUserInformationInBasketCommand
            {
                UserId = "e56ad6b2-210a-4ac0-b7c2-418b4eda9eed",
                Username = "test"
            }, It.IsAny<CancellationToken>());

            actual.Should().BeFalse();

        }
    }
}
