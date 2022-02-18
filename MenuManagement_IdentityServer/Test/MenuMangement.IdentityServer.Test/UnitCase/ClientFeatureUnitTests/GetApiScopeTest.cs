using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using MenuManagement_IdentityServer.AutoMapperProfile;
using MenuManagement_IdentityServer.Models;
using MenuManagement_IdentityServer.Service;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MenuMangement.IdentityServer.Test.UnitCase.ClientFeatureUnitTests
{
    public class GetApiScopeTest : BaseFakeClientDBContext
    {
        public IMapper _mapper;
        public GetApiScopeTest()
        {
            //Seed data
            var ApiScopes = new List<ApiScope>
            {
                new ApiScope { Name = "basketApi" , DisplayName = "basketApi" , Description = "Basket MicroService APIs"},
                new ApiScope { Name = "IdentityServerApi" , DisplayName = "Local API" },
                new ApiScope { Name = "userIDSApi" , DisplayName = "userIDSApi" , Description = "User Controller API in IDS"}
            };

            dbClientContext.ApiScopes.AddRange(ApiScopes);
            dbClientContext.SaveChanges();

            //mapper config
            ApiScopeProfile apiProfile = new ApiScopeProfile();

            var config = new MapperConfiguration(config =>
            {
                config.AddProfile(apiProfile);
            });

            _mapper = new Mapper(config);
        }

        [Fact]
        public void Test_GetAllApiScopes_Should_Get_all_Scopes()
        {
            //Arrange
            var MockClientService = new ClientService(dbClientContext, _mapper);

            //Act
            var result = MockClientService.GetAllApiScopes();

            //Assert
            Assert.True(result.ApiScopes.Count == 3);
        }

        [Fact]
        public void Test_ManageApiScope_Should_Save_Edited_Scope()
        {
            //Arrange
            var MockClientService = new ClientService(dbClientContext, _mapper);
            var model = new GetApiScopeModel 
            {
                Id = 1,
                Name = "basketApi",
                Description = "basketApi description",
                DisplayName = "basketApi"
            };
            //Act
            var result = MockClientService.ManageApiScope(model);

            var apiScope = dbClientContext.ApiScopes.Where(x => x.Id == 1).FirstOrDefault();
            //Assert
            Assert.True(apiScope.Description == "basketApi description");
        }

        [Fact]
        public void Test_ManageApiScope_Should_Save_New_Scope()
        {
            //Arrange
            var MockClientService = new ClientService(dbClientContext, _mapper);
            var model = new GetApiScopeModel
            {
                Id = 0,
                Name = "testApiScope",
                Description = "testApiScope description",
                DisplayName = "testApiScope"
            };
            //Act
            var result = MockClientService.ManageApiScope(model);

            var apiScope = dbClientContext.ApiScopes.Where(x => x.Id == 4).FirstOrDefault();
            //Assert
            Assert.True(apiScope.Name == "testApiScope");
        }
    }
}
