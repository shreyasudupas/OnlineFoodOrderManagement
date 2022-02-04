using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Service;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using MenuManagement_IdentityServer.AutoMapperProfile;
using Newtonsoft.Json;
using MenuManagement_IdentityServer.Models;

namespace MenuMangement.IdentityServer.Test.UnitCase.UserAdministrationUnitTests
{
    public class UserInformationTest : BaseFakeClientDBContext
    {
        private readonly IMapper _mapper;
        public UserInformationTest()
        {
            //Mapper Profile
            var UserProfile = new UserProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(UserProfile);
            });
            _mapper = new Mapper(configuration);
        }

        public void SeedData()
        {
            ApplicationUser newUser = new ApplicationUser
            {
                Id= "00000000-0000-0000-0000-00000000001a",
                UserName = "admin",
                Email = "admin@test.com",
                Address = "sample address , sample address",
                City = "sample city",
                IsAdmin = true,
                ImagePath = "20210112_SampleImage.png",
                CartAmount = 100,
                Points=1
            };

            appContext.Users.Add(newUser);
            appContext.SaveChanges();
        }

        [Fact]
        public void GetUserInformation_Test_When_Passed_UserId()
        {
            //Arrange
            SeedData();
            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, _mapper
                , mockWebHostEnv.Object, logMock.Object);

            var ExpectedModel = new UserInformationModel
            {
                UserId = "00000000-0000-0000-0000-00000000001a",
                Username = "admin",
                Email = "admin@test.com",
                Address = "sample address , sample address",
                City = "sample city",
                ImagePath = "20210112_SampleImage.png",
                CartAmount = 100,
                Points = 1,
                status = MenuManagement_IdentityServer.CrudEnumStatus.success
            };

            var expected = JsonConvert.SerializeObject(ExpectedModel);

            //
            string UserId = "00000000-0000-0000-0000-00000000001a";
            var result = mockService.GetUserInformationDetail(UserId);

            var JsonResult = JsonConvert.SerializeObject(result);
            //Assert
            Assert.Equal(expected, JsonResult);
            
        }

        [Fact]
        public void GetUserInformation_Test_Fail_WhenPassed_UserId_NotPresentInDatabase()
        {
            //Arrange
            SeedData();
            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, _mapper
                , mockWebHostEnv.Object, logMock.Object);

            var ExpectedModel = new UserInformationModel
            {
                status = MenuManagement_IdentityServer.CrudEnumStatus.NotFound,
                ErrorDescription = new List<string>
                {
                    "user not found in database"
                }
            };

            var expected = JsonConvert.SerializeObject(ExpectedModel);

            //
            string UserId = It.IsAny<Guid>().ToString();
            var result = mockService.GetUserInformationDetail(UserId);

            var JsonResult = JsonConvert.SerializeObject(result);
            //Assert
            Assert.Equal(expected, JsonResult);

        }

        [Fact]
        public void GetUserInformation_Test_Fail_DueToMapper_Failure()
        {
            //Arrange
            SeedData();
            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);

            var ExpectedModel = new UserInformationModel
            {
                status = MenuManagement_IdentityServer.CrudEnumStatus.failure,
                ErrorDescription = new List<string>
                {
                    "Mapping Failed"
                }
            };

            var expected = JsonConvert.SerializeObject(ExpectedModel);

            //
            string UserId = "00000000-0000-0000-0000-00000000001a";
            var result = mockService.GetUserInformationDetail(UserId);

            var JsonResult = JsonConvert.SerializeObject(result);
            //Assert
            Assert.Equal(expected, JsonResult);

        }
    }
}
