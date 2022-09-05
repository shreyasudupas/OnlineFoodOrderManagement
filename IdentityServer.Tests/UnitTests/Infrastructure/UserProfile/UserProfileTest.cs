using FluentAssertions;
using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;
using IdentityServer.Infrastruture.Services;
using IdentityServer.Tests.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Infrastructure.UserProfile
{
    public class UserProfileTest : FakeDB
    {
        Mock<UserManager<ApplicationUser>> mockUserManager;
        Mock<ILogger<UserServices>> mockUserProfileLog;
        ApplicationUser newUser;
        UserServices sut;

        public UserProfileTest()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockUserProfileLog = new Mock<ILogger<UserServices>>();

            sut = new UserServices(appContext, mockUserProfileLog.Object, mapper, mockUserManager.Object);
            SeedData();
        }

        public void SeedData()
        {
            newUser = new ApplicationUser
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

            appContext.Users.Add(newUser);

            var userclaim = new IdentityUserClaim<string>
            {
                Id = 1,
                UserId = "00000000-0000-0000-0000-00000000001a",
                ClaimType = "email",
                ClaimValue = "admin@test.com"
            };

            appContext.UserClaims.Add(userclaim);

            appContext.SaveChanges();
        }

        [Fact]
        public async Task GetUserProfileSucccess()
        {
            var actual = await sut.GetUserInformationById("00000000-0000-0000-0000-00000000001a");

            actual.Should().NotBeNull();
            actual.UserName.Should().Be("admin");
        }

        [Fact]
        public async Task GetUserProfile_Failure_Due_To_IncorectUserId()
        {
            var actual = await sut.GetUserInformationById("00000000-0000-0000-0000-00000000002a");

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetUserClaims_Success()
        {
            var claimList = new List<Claim>
            {
                new Claim(type:"email",value:"admin@test.com"),
            };
            mockUserManager.Setup(_ => _.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(claimList);

            var userProfile = new IdenitityServer.Core.Domain.DBModel.UserProfile
            {
                Id = "00000000-0000-0000-0000-00000000001a",
                UserName = "admin",
                Email = "admin@test.com",
                Address = new List<UserProfileAddress>
                {
                    new UserProfileAddress { FullAddress= "sample address , sample address",City = "sample city",State = "sample State",IsActive=true}
                },
                IsAdmin = true,
                ImagePath = "20210112_SampleImage.png",
                CartAmount = 100,
                Points = 1
            };

            var actual = await sut.GetUserClaims(userProfile);
            actual.Claims.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserRoles_Success()
        {
            var roleList = new List<string>
            {
                "appUser","admin"
            };
            mockUserManager.Setup(_ => _.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(roleList);

            var userProfile = new IdenitityServer.Core.Domain.DBModel.UserProfile
            {
                Id = "00000000-0000-0000-0000-00000000001a",
                UserName = "admin",
                Email = "admin@test.com",
                Address = new List<UserProfileAddress>
                {
                    new UserProfileAddress { FullAddress= "sample address , sample address",City = "sample city",State = "sample State",IsActive=true}
                },
                IsAdmin = true,
                ImagePath = "20210112_SampleImage.png",
                CartAmount = 100,
                Points = 1
            };

            var actual = await sut.GetUserRoles(userProfile);
            actual.Roles.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetUserList_Success()
        {
            var actual = await sut.GetUserList();
            actual.Should().HaveCount(1);
            actual[0].UserName.Should().Be("admin");
        }
    }
}
