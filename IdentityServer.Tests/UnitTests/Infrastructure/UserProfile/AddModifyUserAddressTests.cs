using FluentAssertions;
using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;
using IdentityServer.Infrastruture.Services;
using IdentityServer.Tests.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class AddModifyUserAddressTests : FakeDB
    {
        Mock<UserManager<ApplicationUser>> mockUserManager;
        Mock<ILogger<UserServices>> mockUserProfileLog;
        ApplicationUser newUser;
        UserServices sut;

        public AddModifyUserAddressTests()
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
                    new UserAddress { FullAddress= "sample address , sample address",City = "City 1",State = "state 1",Area="area 1",IsActive=true}
                },
                IsAdmin = true,
                ImagePath = "20210112_SampleImage.png",
                CartAmount = 100,
                Points = 1
            };

            appContext.Users.Add(newUser);

            var states = new State
            {
                Id = 1,
                Name = "state 1",
                Cities = new List<City>
                {
                    new City
                    {
                        Id = 1 , Name = "City 1",
                        Areas = new List<LocationArea>
                        {
                            new LocationArea
                            {
                                Id =1 , AreaName = "area 1"
                            }
                        }
                    },
                    new City
                    {
                        Id = 2,
                        Name = "City 2",
                        Areas = new List<LocationArea>
                            {
                                new LocationArea
                                {
                                    Id = 2 , AreaName = "area 2"
                                }
                            }
                    }
                }
            };

            appContext.States.Add(states);

            appContext.SaveChanges();
        }

        [Fact]
        public async Task UserAddress_AddNewAddress_Success()
        {
            var newAddress = new UserProfileAddress
            {
                Id = 0,
                Area = "area 2",
                AreaId = "2",
                City = "City 2",
                CityId = "2",
                FullAddress = "sample 2",
                State = "state 1",
                StateId = "1",
                IsActive = false
            };
            var actual = await sut.AddModifyUserAddress("00000000-0000-0000-0000-00000000001a", newAddress);

            var userAddress = appContext.Users.Include(x => x.Address).ToList();

            userAddress[0].Address.Should().HaveCount(2);
            userAddress[0].Address[1].Area.Should().Be("area 2");
        }

        [Fact]
        public async Task UserAddress_ModifyExistingAddress_Success()
        {
            var newAddress = new UserProfileAddress
            {
                Id = 1,
                Area = "area 2",
                AreaId = "2",
                City = "City 2",
                CityId = "2",
                FullAddress = "sample 2",
                State = "state 1",
                StateId = "1",
                IsActive = false
            };
            var actual = await sut.AddModifyUserAddress("00000000-0000-0000-0000-00000000001a", newAddress);

            var userAddress = appContext.Users.Include(x => x.Address).ToList();

            userAddress[0].Address.Should().HaveCount(1);
            userAddress[0].Address.Should().NotHaveCount(2); //it must not add only update
            userAddress[0].Address[0].Area.Should().Be("area 2");
        }

        [Fact]
        public async Task UserAddress_ModifyAddress_WithUnKnownUserId_Success()
        {
            var newAddress = new UserProfileAddress
            {
                Id = 1,
                Area = "update area",
                City = "update city",
                FullAddress = "update area",
                State = "update state",
                IsActive = false
            };
            var actual = await sut.AddModifyUserAddress("00000000-0000-0000-0000-00000000001b", newAddress);

            actual.Should().BeNull();
        }
    }
}
