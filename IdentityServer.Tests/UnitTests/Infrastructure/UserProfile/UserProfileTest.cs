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
    public class UserProfileTest : FakeDB
    {
        Mock<UserManager<ApplicationUser>> mockUserManager;
        Mock<ILogger<UserServices>> mockUserProfileLog;
        ApplicationUser newUser;

        public UserProfileTest()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockUserProfileLog = new Mock<ILogger<UserServices>>();

            SeedData();

            mockUserManager.Setup(_ => _.FindByIdAsync("00000000-0000-0000-0000-00000000001a"))
                .ReturnsAsync(newUser);

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
            appContext.SaveChanges();
        }

        [Fact]
        public async Task GetUserProfileSucccess()
        {
            var sut = new UserServices(mockUserManager.Object, mockUserProfileLog.Object,mapper);

            var actual = await sut.GetUserInformationById("00000000-0000-0000-0000-00000000001a");

            actual.Should().NotBeNull();
            actual.UserName.Should().Be("admin");
        }
    }
}
