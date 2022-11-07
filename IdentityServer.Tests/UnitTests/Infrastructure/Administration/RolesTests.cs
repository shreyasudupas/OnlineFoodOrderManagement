using FluentAssertions;
using IdentityServer.Infrastruture.Database;
using IdentityServer.Infrastruture.Services;
using IdentityServer.Tests.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Infrastructure.Administration
{
    public class RolesTests : FakeDB
    {
        Mock<RoleManager<IdentityRole>> mockRoleManager;
        AdministrationService sut;
        Mock<ILogger<AdministrationService>> mockLogs;
        public RolesTests()
        {
            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore.Object, null, null, null, null);
            mockLogs = new Mock<ILogger<AdministrationService>>();

            sut = new AdministrationService(mockRoleManager.Object, mockLogs.Object, appContext, dbClientContext);

            SeedData(appContext);
        }

        private void SeedData(ApplicationDbContext appContext)
        {
            appContext.Roles.Add(new IdentityRole
            {
                Name="appLooser",
            });

            appContext.SaveChanges();

        }

        [Fact]
        public void GetRoles_Success()
        {
            var mockRolesResult = new List<IdentityRole>
            {
                new IdentityRole{ Id= "1" ,Name="user" },
                new IdentityRole {Id="2",Name ="admin"}
            };
            var mockRoles = mockRolesResult.AsQueryable();
            mockRoleManager.Setup(_ => _.Roles).Returns(mockRoles);

            var actual = sut.Roles();
            actual.Should().HaveCount(2);
            actual[0].RoleId.Should().Be("1");
        }

        [Fact]
        public async Task AddNewRole_Success()
        {
            mockRoleManager.Setup(_ => _.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var actual = await sut.AddRole(new IdenitityServer.Core.Domain.Response.RoleListResponse { RoleName = "appLooser" });
            actual.Should().NotBeNull();
            actual.RoleName.Should().Be("appLooser");
        }

        [Fact]
        public async Task AddNewRole_Failure_BecauseOfAlreadyPresentRole()
        {
            mockRoleManager.Setup(_ => _.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description ="admin is already present" }));

            var actual = await sut.AddRole(new IdenitityServer.Core.Domain.Response.RoleListResponse { RoleName = "admin" });
            actual.Should().BeNull();
            mockLogs.Verify(
                x => x.Log(It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Role Save Errors: [{\"Code\":null,\"Description\":\"admin is already present\"}]"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                ));
        }
    }
}
