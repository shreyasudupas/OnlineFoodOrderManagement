using FluentAssertions;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Mediators.Login;
using IdentityServer.Infrastruture.Database;
using IdentityServer.Tests.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Infrastructure.AuthService
{
    public class LoginFeature : FakeDB
    {

        Mock<UserManager<ApplicationUser>> mockUserManager;
        Mock<SignInManager<ApplicationUser>> mockSignInManager;
        bool signInMethodCalled;
        Infrastruture.Services.AuthService sut;

        public LoginFeature()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager.Object,new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,null,null,null,null);

            sut = new Infrastruture.Services.AuthService(mockSignInManager.Object, mockUserManager.Object);

            ApplicationUser newUser = new ApplicationUser
            {
                Id = "00000000-0000-0000-0000-00000000001a",
                UserName = "admin",
                Email = "admin@test.com",
                Address = new List<UserAddress>
                {
                    new UserAddress { FullAddress= "sample address , sample address",City = "sample city",State = "sample State",IsActive=true}
                },
                IsAdmin = true,
                ImagePath = "20210112_SampleImage.png",
                CartAmount = 100,
                Points = 1
            };

            mockUserManager.Setup(x => x.CreateAsync(newUser, "password"))
                .ReturnsAsync(IdentityResult.Success);
            mockSignInManager.Setup(x => x.PasswordSignInAsync("admin", "password", false, false))
                .ReturnsAsync(SignInResult.Success);
            mockSignInManager.Setup(x => x.PasswordSignInAsync("user", "password", false, false))
                .ReturnsAsync(SignInResult.Failed);
            mockUserManager.Setup(x => x.FindByNameAsync("admin")).ReturnsAsync(newUser);
            mockSignInManager.Setup(x => x.SignInAsync(newUser, false,null))
                .Callback(() => signInMethodCalled = true);
        }

        [Fact]
        public async Task Login_Success()
        {
            var request = new LoginCommand
            {
                Username = "admin",
                Password = "password"
            };
            var actual = await sut.Login(request);

            actual.Should().NotBeNull();
            actual.Error.Should().BeNullOrEmpty();
            actual.RedirectRequired.Should().BeFalse();
            signInMethodCalled.Should().BeTrue();

        }

        [Fact]
        public async Task Login_Failure()
        {
            var request = new LoginCommand
            {
                Username = "user",
                Password = "password"
            };
            var actual = await sut.Login(request);

            actual.Should().NotBeNull();
            actual.Error.Should().Be("Incorrect Username/Password");
            actual.RedirectRequired.Should().BeFalse();
            signInMethodCalled.Should().BeFalse();
        }

        [Fact]
        public async Task Login_SuccessWithRedirectionRequired()
        {
            var request = new LoginCommand
            {
                Username = "admin",
                Password = "password",
                ReturnUrl="https://google.com"
            };
            var actual = await sut.Login(request);

            actual.Should().NotBeNull();
            actual.Error.Should().BeNullOrEmpty();
            actual.RedirectRequired.Should().BeTrue();
            signInMethodCalled.Should().BeTrue();

        }
    }
}
