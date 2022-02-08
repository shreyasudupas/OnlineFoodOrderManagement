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
using MenuManagement_IdentityServer;

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
                Address = new List<UserAddress>
                {
                    new UserAddress { FullAddress= "sample address , sample address",City = "sample city",State = "sample State",IsActive=true}
                },
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
                Address = new List<UserAdressModel>
                {
                    new UserAdressModel { FullAddress = "sample address , sample address" , City = "sample city",State = "sample State",IsActive=true }
                },
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

        [Fact]
        public void SaveUserAddress_Function_Success()
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


            //
            var model = new UserAddressPartialViewModel
            {
                City = "sample city",
                State = "sample State",
                FullAddress = "sample street, sample street",
                UserId = "00000000-0000-0000-0000-00000000001a",
                IsActive = false
            };
            var result = mockService.SaveUserAddress(model);

            //Assert
            Assert.Equal(Enum.GetName(CrudEnumStatus.success), Enum.GetName(result.status));

        }

        [Fact]
        public void SaveUserAddress_Function_Falure_if_Added_more_than_one_active_address()
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


            //
            var model = new UserAddressPartialViewModel
            {
                City = "sample city",
                State = "sample State",
                FullAddress = "sample street, sample street",
                UserId = "00000000-0000-0000-0000-00000000001a",
                IsActive = true
            };
            var result = mockService.SaveUserAddress(model);

            //Assert
            Assert.Equal(Enum.GetName(CrudEnumStatus.failure), Enum.GetName(result.status));
        }

        [Fact]
        public void SaveUserAddress_Function_AddUser_Address_Not_Found()
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


            //
            var model = new UserAddressPartialViewModel
            {
                City = "sample city",
                State = "sample State",
                FullAddress = "sample street, sample street",
                UserId = "00000000-0000-0000-0000-000000000012a",
                IsActive = true
            };
            var result = mockService.SaveUserAddress(model);

            //Assert
            Assert.Equal(Enum.GetName(CrudEnumStatus.NotFound), Enum.GetName(result.status));
        }

        [Fact]
        public void SaveUserAddress_Function_Edit_User_When_Passed_UserAddressId_Success()
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


            //
            var model = new UserAddressPartialViewModel
            {
                UserAddressId = 1,
                City = "sample city 1",
                State = "sample State 1",
                FullAddress = "sample street, sample street",
                UserId = "00000000-0000-0000-0000-00000000001a",
                IsActive = false
            };
            var result = mockService.SaveUserAddress(model);

            //Assert
            Assert.Equal(Enum.GetName(CrudEnumStatus.success), Enum.GetName(result.status));

        }

        [Fact]
        public void SaveUserAddress_Function_Edit_User_When_Passed_Invalid_UserAddressId_NotFound()
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


            //
            var model = new UserAddressPartialViewModel
            {
                UserAddressId = 2,
                City = "sample city 1",
                State = "sample State 1",
                FullAddress = "sample street, sample street",
                UserId = "00000000-0000-0000-0000-00000000001a",
                IsActive = false
            };
            var result = mockService.SaveUserAddress(model);

            //Assert
            Assert.Equal(Enum.GetName(CrudEnumStatus.NotFound), Enum.GetName(result.status));
            Assert.Equal("UserAddress not found", result.ErrorDescription[0]);
        }

        [Fact]
        public void SaveUserAddress_Function_Edit_User_Should_fail_If_More_than_One_Active_Address()
        {
            //Arrange
            ApplicationUser newUser = new ApplicationUser
            {
                Id = "00000000-0000-0000-0000-00000000001a",
                UserName = "admin",
                Email = "admin@test.com",
                Address = new List<UserAddress>
                {
                    new UserAddress { FullAddress= "sample address , sample address 1",City = "sample city",State = "sample State",IsActive=true},
                    new UserAddress { FullAddress= "sample address , sample address 2",City = "sample city 2",State = "sample State 2",IsActive=false}
                },
                IsAdmin = true,
                ImagePath = "20210112_SampleImage.png",
                CartAmount = 100,
                Points = 1
            };

            appContext.Users.Add(newUser);
            appContext.SaveChanges();

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


            //
            var model = new UserAddressPartialViewModel
            {
                UserAddressId = 2,
                City = "sample city 21",
                State = "sample State 21",
                FullAddress = "sample street, sample street",
                UserId = "00000000-0000-0000-0000-00000000001a",
                IsActive = true
            };
            var result = mockService.SaveUserAddress(model);

            //Assert
            Assert.Equal(Enum.GetName(CrudEnumStatus.failure), Enum.GetName(result.status));
            Assert.Equal("User has more than two active address", result.ErrorDescription[0]);
        }
    }
}
