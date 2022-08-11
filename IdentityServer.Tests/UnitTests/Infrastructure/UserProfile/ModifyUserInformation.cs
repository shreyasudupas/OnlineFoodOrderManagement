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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Infrastructure.UserProfile
{
    public class ModifyUserInformation : FakeDB
    {
        Mock<UserManager<ApplicationUser>> mockUserManager;
        Mock<ILogger<UserServices>> mockUserProfileLog;
        ApplicationUser newUser;
        UserServices sut;

        public ModifyUserInformation()
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
        public async Task ModifyUserInfo_Success()
        {
            
            var mockUser = new IdenitityServer.Core.Domain.DBModel.UserProfile
            {
                Id = "00000000-0000-0000-0000-00000000001a",
                UserName = "admin",
                Email = "test@test.com",
                IsAdmin = true,
                Points = 1,
                CartAmount = 1500
            };
            mockUserManager.Setup(_ => _.GetUserIdAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("00000000-0000-0000-0000-00000000001a");

            mockUserManager.Setup(_ => _.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await sut.ModifyUserInformation(mockUser);

            result.Should().BeTrue();
        }
    }
}
