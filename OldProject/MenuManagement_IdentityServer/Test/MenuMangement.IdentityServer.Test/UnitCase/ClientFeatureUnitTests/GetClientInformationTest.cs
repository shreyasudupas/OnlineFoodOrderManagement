using Moq;
using System.Collections.Generic;
using IdentityServer4.EntityFramework.Entities;
using MenuManagement_IdentityServer.Service;
using Xunit;
using AutoMapper;
using MenuMangement.IdentityServer.Test.Helper.FakeDbContext;
using MenuManagement_IdentityServer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        [Fact]
        public void ManageAllowedScope_Test_Add_New_Scope_AndRemove_Existing_Scope_Success()
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
                        new ClientScope { Id = 1, Scope="IdentityServerApi" },
                        new ClientScope { Id = 2, Scope="userIDSApi" }
                    } //cannot have openId for client credential flow
                 },
            };

            dbClientContext.Clients.AddRange(Clients);
            dbClientContext.SaveChanges();

            ClientViewModel model = new ClientViewModel();
            //add profile scope and remove rest
            model.AllowedScopeSelected.Add("profile");

            var clientId = dbClientContext.Clients.Select(x=>x.ClientId).FirstOrDefault();

            //act
             ClientServiceMock.ManageAllowedScope(model, clientId);

            var Client = dbClientContext.Clients.Include(x => x.AllowedScopes).Where(c => c.ClientId == clientId).FirstOrDefault();

            //Assert
            Assert.True(Client.AllowedScopes.Count == 1);

        }

        [Fact]
        public void ManageAllowedScope_Test_Remove_Existing_Scope_Success()
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
                        new ClientScope { Id = 1, Scope="IdentityServerApi" },
                        new ClientScope { Id = 2, Scope="userIDSApi" }
                    } //cannot have openId for client credential flow
                 },
            };

            dbClientContext.Clients.AddRange(Clients);
            dbClientContext.SaveChanges();

            ClientViewModel model = new ClientViewModel();

            //Remove IdentityServerApi scope
            model.AllowedScopeSelected.Add("userIDSApi");

            var clientId = dbClientContext.Clients.Select(x => x.ClientId).FirstOrDefault();

            //act
            ClientServiceMock.ManageAllowedScope(model, clientId);

            var Client = dbClientContext.Clients.Include(x => x.AllowedScopes).Where(c => c.ClientId == clientId).FirstOrDefault();

            //Assert
            Assert.True(Client.AllowedScopes.Count == 1);
            Assert.Equal("userIDSApi", Client.AllowedScopes[0].Scope);
        }

        [Fact]
        public void ManageAllowedScope_Test_Add_OneScope_Remove_One_Scope_Success()
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
                        new ClientScope { Id = 1, Scope="IdentityServerApi" },
                        new ClientScope { Id = 2, Scope="userIDSApi" }
                    } //cannot have openId for client credential flow
                 },
            };

            dbClientContext.Clients.AddRange(Clients);
            dbClientContext.SaveChanges();

            ClientViewModel model = new ClientViewModel();

            //means IdentityServerApi scope is unchecked as is to be removed
            model.AllowedScopeSelected.Add("userIDSApi");
            model.AllowedScopeSelected.Add("newScope");

            var clientId = dbClientContext.Clients.Select(x => x.ClientId).FirstOrDefault();

            //act
            ClientServiceMock.ManageAllowedScope(model, clientId);

            var Client = dbClientContext.Clients.Include(x => x.AllowedScopes).Where(c => c.ClientId == clientId).FirstOrDefault();

            //Assert
            Assert.True(Client.AllowedScopes.Count == 2);
            Assert.Contains("newScope", Client.AllowedScopes[1].Scope);

        }

        [Fact]
        public void ManageGrantType_Test_Add_OneGrantType_Success()
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
                    AllowedGrantTypes = new List<ClientGrantType>(),
                    AllowedScopes = new List<ClientScope>
                    {
                        new ClientScope { Id = 1, Scope="IdentityServerApi" },
                        new ClientScope { Id = 2, Scope="userIDSApi" }
                    } //cannot have openId for client credential flow
                 },
            };

            dbClientContext.Clients.AddRange(Clients);
            dbClientContext.SaveChanges();

            ClientViewModel model = new ClientViewModel();

            //add one grant type
            model.GrantTypesSelected.Add("client_credentials");

            var clientId = dbClientContext.Clients.Select(x => x.ClientId).FirstOrDefault();

            //act
            ClientServiceMock.ManageGrantType(model, clientId);

            var Client = dbClientContext.Clients.Include(x => x.AllowedGrantTypes).Where(c => c.ClientId == clientId).FirstOrDefault();

            //Assert
            Assert.True(Client.AllowedGrantTypes.Count == 1);
            Assert.Contains("client_credentials", Client.AllowedGrantTypes[0].GrantType);

        }

        [Fact]
        public void ManageGrantType_Test_Remove_OneGrantType_Success()
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
                        new ClientGrantType { Id =1 , GrantType = "client_credentials"},
                        new ClientGrantType { Id =2 , GrantType = "client_credentials 2"}
                    },
                    AllowedScopes = new List<ClientScope>
                    {
                        new ClientScope { Id = 1, Scope="IdentityServerApi" },
                        new ClientScope { Id = 2, Scope="userIDSApi" }
                    } //cannot have openId for client credential flow
                 },
            };

            dbClientContext.Clients.AddRange(Clients);
            dbClientContext.SaveChanges();

            ClientViewModel model = new ClientViewModel();

            //add one grant type
            model.GrantTypesSelected.Add("client_credentials");

            var clientId = dbClientContext.Clients.Select(x => x.ClientId).FirstOrDefault();

            //act
            ClientServiceMock.ManageGrantType(model, clientId);

            var Client = dbClientContext.Clients.Include(x => x.AllowedGrantTypes).Where(c => c.ClientId == clientId).FirstOrDefault();

            //Assert
            Assert.True(Client.AllowedGrantTypes.Count == 1);
            Assert.Contains("client_credentials", Client.AllowedGrantTypes[0].GrantType);

        }

        [Fact]
        public void ManageGrantType_Test_Remove_OneGrantType_And_AddOneGrantType_Success()
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
                        new ClientGrantType { Id =1 , GrantType = "client_credentials"},
                        new ClientGrantType { Id =2 , GrantType = "client_credentials 2"}
                    },
                    AllowedScopes = new List<ClientScope>
                    {
                        new ClientScope { Id = 1, Scope="IdentityServerApi" },
                        new ClientScope { Id = 2, Scope="userIDSApi" }
                    } //cannot have openId for client credential flow
                 },
            };

            dbClientContext.Clients.AddRange(Clients);
            dbClientContext.SaveChanges();

            ClientViewModel model = new ClientViewModel();

            //add one grant type
            model.GrantTypesSelected.Add("client_credentials");
            model.GrantTypesSelected.Add("code");

            var clientId = dbClientContext.Clients.Select(x => x.ClientId).FirstOrDefault();

            //act
            ClientServiceMock.ManageGrantType(model, clientId);

            var Client = dbClientContext.Clients.Include(x => x.AllowedGrantTypes).Where(c => c.ClientId == clientId).FirstOrDefault();

            //Assert
            Assert.True(Client.AllowedGrantTypes.Count == 2);
            Assert.Contains("code", Client.AllowedGrantTypes[1].GrantType);

        }
    }
}
