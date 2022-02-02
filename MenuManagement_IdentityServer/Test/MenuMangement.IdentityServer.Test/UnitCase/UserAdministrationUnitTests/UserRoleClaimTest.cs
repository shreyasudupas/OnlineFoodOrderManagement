using AutoMapper;
using MenuManagement_IdentityServer.Data.Models;
using MenuManagement_IdentityServer.Service;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
                Address = "sample address , sample address",
                City = "sample city",
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

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new RoleManager<IdentityRole>(mockRoleStore.Object, null, null, null, null);
            var mockIMapper = new Mock<IMapper>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();
            var logMock = new Mock<ILogger<UserAdministrationManager>>();

            var mockService = new UserAdministrationManager(mockUserManager, mockRoleManager, appContext, mockIMapper.Object
                , mockWebHostEnv.Object, logMock.Object);

            var NewRoles = new List<string> { "admin" };
            var ExistingRoles = new List<string>();

            var user = appContext.Users.FirstOrDefault();
            var claim = new Claim("test","test");

            mockUserStoreClaim.Setup(x => x.AddClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<Claim>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(IdentityResult.Success));

             mockService.ManageRoleClaim(NewRoles,true,false, user, ExistingRoles);
        }
    }
}
