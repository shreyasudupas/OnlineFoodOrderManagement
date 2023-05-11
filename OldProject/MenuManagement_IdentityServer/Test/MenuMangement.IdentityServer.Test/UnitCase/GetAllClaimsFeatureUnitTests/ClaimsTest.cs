using AutoMapper;
using MenuManagement_IdentityServer;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MenuMangement.IdentityServer.Test.UnitCase.GetAllClaimsFeatureUnitTests
{
    public class ClaimsTest : BaseFakeClientDBContext
    {
        private void SeedData()
        {
            var ListClaimsSelections = new List<ClaimDropDown>
                            {
                                new ClaimDropDown { Name = "Username", Value="userName"},
                                new ClaimDropDown { Name = "Email", Value="email" },
                                new ClaimDropDown { Name = "Role",Value="role"}
                            };

            appContext.ClaimDropDowns.AddRange(ListClaimsSelections);
            appContext.SaveChanges();
        }

        //[Fact]
        public void UserMangerAndRoleManger_IfUsed_DirectMock_Should_GivenInstanceNotFoundError()
        {
            //Arrange
            SeedData();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>();

            var mockRoleManager = new Mock<RoleManager<IdentityRole>>();
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            //Act
            Action act = () => new UserAdministrationManager(mockUserManager.Object, mockRoleManager.Object, appContext, mockIMapper.Object, mockWebHostEnv.Object, logMock.Object);
            var result = Assert.Throws<Castle.DynamicProxy.InvalidProxyConstructorArgumentsException>(() =>  act);

            //Assert
           
        }

        [Fact]
        public void GetAllDropDownClaims_Must_Return_ListOfClaimsTest()
        {
            //Arrange
            SeedData();
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object,null,null,null,null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext,mockIMapper.Object
                ,mockWebHostEnv.Object,logMock.Object);

            //Act
            var result = mockService.GetAllDropDownClaims();

            //Assert
            Assert.Equal(3, result.Claims.Count);
        }

        [Fact]
        public void Should_Add_NewClaim_In_ClaimsDropDown()
        {
            //Arrange
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);
            var model = new EditClaimViewModel
            {
                ClaimType = "Profile Pic",
                ClaimValue = "picture"
            };

            //Act
            var result = mockService.EditClaim(model);

            //Assert
            Assert.Contains(Enum.GetName(CrudEnumStatus.success), Enum.GetName(result.status));
        }

        [Fact]
        public void Should_Edit_ExistingClaim_In_ClaimsDropDown()
        {
            //Arrange
            SeedData();
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);
            var model = new EditClaimViewModel
            {
                ClaimId = 1,
                ClaimType = "Username",
                ClaimValue = "username"
            };

            //Act
            var result = mockService.EditClaim(model);

            //Assert
            Assert.Contains(Enum.GetName(CrudEnumStatus.success), Enum.GetName(result.status));
        }

        [Fact]
        public void Should_Give_Error_WhenNonExistantClaim_IsAdded_In__ClaimsDropDown()
        {
            //Arrange
            SeedData();
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);
            var model = new EditClaimViewModel
            {
                ClaimId = 4,
                ClaimType = "Pitcure url",
                ClaimValue = "pitcher"
            };

            //Act
            var result = mockService.EditClaim(model);

            //Assert
            Assert.Contains(Enum.GetName(CrudEnumStatus.failure), Enum.GetName(result.status));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Give_Claim_By_Id_ClaimsDropDown(int id)
        {
            //Arrange
            SeedData();
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);
            

            //Act
            var result = mockService.GetClaimById(id);

            //Assert
            Assert.Contains(Enum.GetName(CrudEnumStatus.success), Enum.GetName(result.status));
        }

        [Theory]
        [InlineData(5)]
        public void Should_Give_Error_Claim_WhenPassed_Null_Or_NonExsistantId_ClaimsDropDown(int? id)
        {
            //Arrange
            SeedData();
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);


            //Act
            var result = mockService.GetClaimById(id);

            //Assert
            Assert.Contains(Enum.GetName(CrudEnumStatus.failure), Enum.GetName(result.status));
        }
    }
}
