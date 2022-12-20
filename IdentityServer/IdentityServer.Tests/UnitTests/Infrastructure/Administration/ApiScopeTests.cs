using FluentAssertions;
using IdenitityServer.Core.Domain.Model;
using IdentityServer.Infrastruture.Services;
using IdentityServer.Tests.Helper;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Infrastructure.Administration
{
    public class ApiScopeTests : FakeDB
    {
        Mock<RoleManager<IdentityRole>> mockRoleManager;
        AdministrationService sut;
        Mock<ILogger<AdministrationService>> mockLogs;

        public ApiScopeTests()
        {
            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore.Object, null, null, null, null);
            mockLogs = new Mock<ILogger<AdministrationService>>();

            sut = new AdministrationService(mockRoleManager.Object, mockLogs.Object, appContext, dbClientContext);
            SeedData();
        }

        private void SeedData()
        {
            var apiScopeList = new List<ApiScope>
            {
                new ApiScope{ Id =1 , Name ="BasketApi",Description="test",DisplayName="Basket API"},
                new ApiScope{ Id =2 , Name ="MenuApi",Description="test",DisplayName="Menu API"}
            };

            dbClientContext.ApiScopes.AddRange(apiScopeList);
            dbClientContext.SaveChanges();
        }

        [Fact]
        public async Task GetAllApiScopesSuccess()
        {
            var actual = await sut.GetApiScopes();
            actual.Should().NotBeEmpty();
            actual[0].Name.Should().Be("BasketApi");
        }

        [Fact]
        public async Task Add_ApiScopes_Success()
        {
            var newApiScope = new ApiScopeModel
            {
                Name = "newApi",
                Description="new Api Desc",
                DisplayName = "New"
            };
            var actual = await sut.AddApiScope(newApiScope);
            actual.Should().NotBeNull();
            
        }

        [Fact]
        public async Task Save_ApiScopes_Success()
        {
            var newApiScope = new ApiScopeModel
            {
                Id = 1,
                Name = "BasketApi",
                Description = "new Basket Desc",
                DisplayName = "New Basket"
            };
            var actual = await sut.SaveApiScope(newApiScope);
            actual.Should().NotBeNull();

            var editScope = dbClientContext.ApiScopes.Where(a => a.Id == 1).FirstOrDefault();
            editScope.Description.Should().Be(newApiScope.Description);
        }

        [Fact]
        public async Task Save_ApiScopes_Failure_Due_To_IncorrectID()
        {
            var newApiScope = new ApiScopeModel
            {
                Id = 5,
                Name = "BasketApi",
                Description = "new Basket Desc",
                DisplayName = "New Basket"
            };
            var actual = await sut.SaveApiScope(newApiScope);
            actual.Should().BeNull();

        }
    }
}
