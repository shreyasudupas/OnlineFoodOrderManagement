﻿using FluentAssertions;
using IdentityServer.Infrastruture.Services;
using IdentityServer.Tests.Helper;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.Tests.UnitTests.Infrastructure.Clients
{
    public class ClientServiceTests: FakeDB
    {
        ClientServices sut;
        Mock<ILogger<ClientServices>> mockLogger;
        public ClientServiceTests()
        {
            mockLogger = new Mock<ILogger<ClientServices>>();
            sut = new ClientServices(dbClientContext, mockLogger.Object);

            SeedData();
        }

        private void SeedData()
        {
            var clients = new List<Client>
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
                new Client
                {
                    ClientId = "test client 2",
                    ClientName = "Client name 2",
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret { Id = 2 , Value="Secret"}
                    },
                    AllowedGrantTypes = new List<ClientGrantType>
                    {
                        new ClientGrantType { Id =2 , GrantType = "Test grant 1"},
                        new ClientGrantType { Id =3 , GrantType = "Test grant 2"}
                    },
                    AllowedScopes = new List<ClientScope>
                    {
                        new ClientScope { Id = 2, Scope="testApi"},
                        new ClientScope { Id = 3, Scope="testApi 2"}
                    } 
                 },
            };
            dbClientContext.Clients.AddRange(clients);
            dbClientContext.SaveChanges();
        }

        [Fact]
        public async Task GetAllClientsTest_Success()
        {
            var result = await sut.GetAllClients();
            result.Should().HaveCount(2);
            result[1].ClientId.Should().Be("test client 2");
        }

        [Fact]
        public async Task GetClientByID_Success()
        {
            var result = await sut.GetClientById(1);
            result.Should().NotBeNull();
            result.AllowedScopes.Should().HaveCount(1);
        }

        [Fact]
        public async Task Save_ClientAllowedScopes_AddNewScope_AndRemoveOneScope()
        {
            var selectedScopes = new List<string> { "testApi", "newScope" };
            var result = await sut.SaveAllowedScopes(2, selectedScopes);
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Save_ClientAllowedScopes_NoSaveShouldHappen_IfSameScopeIsSelected()
        {
            var selectedScopes = new List<string> { "testApi", "testApi 2" };
            var result = await sut.SaveAllowedScopes(2, selectedScopes);
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Save_ClientAllowedScopes_DeleteAllScope()
        {
            var selectedScopes = new List<string> ();
            var result = await sut.SaveAllowedScopes(2, selectedScopes);
            result.Should().BeEmpty();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task Save_ClientAllowedGrantTypes_AddNewGrant_AndRemoveFirstGrant()
        {
            var selectedGrantTypes = "Test grant 2";
            var result = await sut.SaveAllowedGrants(1, selectedGrantTypes);
            result.Should().NotBeEmpty();
            result.Should().Be("Test grant 2");
        }

        [Fact]
        public async Task Save_ClientAllowedGrantTypes_SendQueryBack()
        {
            var selectedGrantTypes = "client_credentials";
            var result = await sut.SaveAllowedGrants(1, selectedGrantTypes);
            result.Should().NotBeEmpty();
            result.Should().Be("client_credentials");
        }
    }
}
