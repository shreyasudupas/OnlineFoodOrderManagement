using FluentAssertions;
using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Features.Register;
using IdentityServer.Infrastruture.Database;
using IdentityServer.Tests.Helper;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Infrastructure.AuthService
{
    public class RegisterFeature : FakeDB
    {
        Mock<UserManager<ApplicationUser>> mockUserManager;
        Mock<SignInManager<ApplicationUser>> mockSignInManager;
        bool signInMethodCalled;
        Infrastruture.Services.AuthService sut;
        Mock<IIdentityServerInteractionService> mockIdentityService;
        Mock<ILogger<IdentityServer.Infrastruture.Services.AuthService>> mockLog;
        Mock<IAddressService> mockAddressService;

        public RegisterFeature()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager.Object, new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, null, null, null, null);

            mockIdentityService = new Mock<IIdentityServerInteractionService>();
            mockLog = new Mock<ILogger<Infrastruture.Services.AuthService>>();
            mockAddressService = new Mock<IAddressService>();

            sut = new Infrastruture.Services.AuthService(mockSignInManager.Object, mockUserManager.Object, mockIdentityService.Object
                , mockLog.Object, appContext, mockAddressService.Object);

            MigrateDatabase.MapAddressStateLocation(appContext);
        }

        [Fact]
        public async Task Register_User_Success()
        {
            mockUserManager.Setup(_ => _.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(_ => _.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(_ => _.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var command = new RegisterCommand
            {
                Username = "test",
                Password = "testpass",
                Address = "test address",
                CityId = "1",
                StateId ="1",
                AreaId="1",
                Email="test.com"
            };
            await sut.Register(command);

            command.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Register_User_Failure()
        {
            var errors = new IdentityError[] { new IdentityError { Code = "400" , Description ="Username already present" } };
            mockUserManager.Setup(_ => _.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));
            

            var command = new RegisterCommand
            {
                Username = "test",
                Password = "testpass",
                Address = "test address",
                CityId = "1",
                StateId = "1",
                AreaId="1",
                Email = "test.com"
            };
            await sut.Register(command);

            command.Errors.Should().SatisfyRespectively(first=> 
            {
                first.Should().Be("Username already present");
            });
        }
    }
}
