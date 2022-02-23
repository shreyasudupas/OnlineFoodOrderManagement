using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Service;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace MenuMangement.IdentityServer.Test.UnitCase.UserAdministrationUnitTests
{
    public class UserRoleClaimTest : BaseFakeClientDBContext
    {
        public void SeedData()
        {
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@test.com",
                Address = new List<UserAddress>
                {
                    new UserAddress { FullAddress= "sample address , sample address",City = "sample city",State = "sample State",IsActive=true}
                },
                IsAdmin = true
            };

            appContext.Users.Add(newUser);
            appContext.SaveChanges();
        }

        [Fact]
        public void Test_User_Role_Claim_Success_WhenAddingRole_ForTheFirstTime()
        {
            //Arrange
            SeedData();
            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStoreClaim.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager.Object, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);

            var NewRoles = new List<string> { "admin" };
            var ExistingRoles = new List<string>();

            var user = appContext.Users.FirstOrDefault();

            mockUserManager.Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

             mockService.ManageRoleClaim(NewRoles,true,false, user, ExistingRoles);
        }

        [Fact]
        public void ManageRoleClaim_Should_Be_True_If_multiple_Role_is_Added()
        {
            //Arrange
            SeedData();

            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStoreClaim.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager.Object, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);

            var NewRoles = new List<string> { "appUser" };
            var ExistingRoles = new List<string> { "admin" };

            var user = appContext.Users.FirstOrDefault();

            var GetClaimsList = new List<Claim>
            {
               new Claim("role","admin")
            };

            mockUserManager.Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);


            mockService.ManageRoleClaim(NewRoles, true, false, user, ExistingRoles);
        }

        [Fact]
        public void ManageRoleClaim_Remove_OnlyRole_With_The_User()
        {
            //Arrange
            SeedData();

            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStoreClaim.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager.Object, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);

            var NewRoles = new List<string> ();
            var ExistingRoles = new List<string> { "admin" };

            var user = appContext.Users.FirstOrDefault();

            var GetClaimsList = new List<Claim>
            {
               new Claim("role","admin")
            };

            mockUserManager.Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(GetClaimsList);

            mockUserManager.Setup(x => x.RemoveClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            mockService.ManageRoleClaim(NewRoles, false, true, user, ExistingRoles);

        }

        [Fact]
        public void ManageRoleClaim_Remove_OneOf_the_Role_Of_The_User()
        {
            //Arrange
            SeedData();

            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStoreClaim.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager.Object, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);

            var RemoveRoles = new List<string> { "appUser" };
            var ExistingRoles = new List<string> { "admin","appUser" };

            var user = appContext.Users.FirstOrDefault();

            var GetClaimsList = new List<Claim>
            {
               new Claim("role","admin,appUser"),
            };

            mockUserManager.Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(GetClaimsList);

            mockUserManager.Setup(x => x.RemoveClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            mockService.ManageRoleClaim(RemoveRoles, false, true, user, ExistingRoles);

        }

        [Fact]
        public void ManageRoleClaim_Remove_OneOf_the_Role_Of_The_User_From_List_of_Roles()
        {
            //Arrange
            SeedData();

            var mockUserStoreClaim = new Mock<IUserClaimStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStoreClaim.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager.Object, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);

            var RemoveRoles = new List<string> { "appUser" };
            var ExistingRoles = new List<string> { "admin", "appUser" ,"testUsers"};

            var user = appContext.Users.FirstOrDefault();

            var GetClaimsList = new List<Claim>
            {
               new Claim("role","admin,appUser,testUsers"),
            };

            mockUserManager.Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(GetClaimsList);

            mockUserManager.Setup(x => x.RemoveClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            mockService.ManageRoleClaim(RemoveRoles, false, true, user, ExistingRoles);

        }
    }
}
