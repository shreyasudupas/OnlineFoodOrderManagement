using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using MenuManagement_IdentityServer.Service;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MenuMangement.IdentityServer.Test.UnitCase.ClientFeature
{
    public class EditDeleteFeatureClientServiceTest : BaseFakeClientDBContext
    {
        public EditDeleteFeatureClientServiceTest()
        {
            SeedClientData();
        }

        private void SeedClientData()
        {
            var Clients = new List<Client>
            {
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret { Id = 1 , Value="Secret"}
                    },
                    AllowedGrantTypes = new List<ClientGrantType>
                    {
                        new ClientGrantType { Id =1 , GrantType = "client_credentials"}
                    },
                    AllowedScopes = new List<ClientScope>
                    {
                        new ClientScope { Id = 1, Scope="basketApi"}
                    } //cannot have openId for client credential flow
                 },
            };

            dbClientContext.Clients.AddRange(Clients);
            dbClientContext.SaveChanges();
        }

        [Theory]
        [InlineData("m2m.client")]
        public void Test_Delete_Client_When_Passing_ClientId(string clientId)
        {
            //Arrange
            var MockMapper = new Mock<IMapper>();
            var mockService = new ClientService(dbClientContext,MockMapper.Object);
            //Act
            var result = mockService.DeleteClient(clientId);
            //Assert
            Assert.True(result.status == MenuManagement_IdentityServer.CrudEnumStatus.success);
        }
    }
}
