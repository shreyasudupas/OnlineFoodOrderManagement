using Moq;
using System.Collections.Generic;
using IdentityServer4.EntityFramework.Entities;
using MenuManagement_IdentityServer.Service;
using Xunit;
using AutoMapper;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;

namespace MenuMangement.IdentityServer.Test.UnitCase.ClientFeatureUnitTests
{
    public class GetClientInformationTest: BaseFakeClientDBContext
    {
        public GetClientInformationTest()
        {
        }
        
        [Fact]
        public void TestGetAllClient_Service_Contains_MoreThanOneClient()
        {
            //arrange
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            var ClientServiceMock = new ClientService(dbClientContext, mockMapper.Object);

            //add all clients
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

            //act
            var result =  ClientServiceMock.GetAllClient().GetAwaiter().GetResult();

            //assert
            Assert.True(result.Data.Count > 0);
        }
    }
}
