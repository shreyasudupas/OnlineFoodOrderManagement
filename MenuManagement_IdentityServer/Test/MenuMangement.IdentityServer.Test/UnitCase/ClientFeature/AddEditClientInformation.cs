using IdentityServer4.EntityFramework.DbContexts;
using Moq;
using System.Collections.Generic;
using IdentityServer4.EntityFramework.Entities;
using MenuManagement_IdentityServer.Service;
using Microsoft.EntityFrameworkCore;

namespace MenuMangement.IdentityServer.Test.UnitCase.ClientFeature
{
    public class AddEditClientInformation
    {
        public Mock<ClientService> clientSerive;
        public Mock<ConfigurationDbContext> dbClientContext;
        public AddEditClientInformation()
        {
            clientSerive = new Mock<ClientService>();
        }
        public void DeleteClientRedirectUrl_Test()
        {

        }

        public void ClientSetup()
        {
            //dbClientContext = new Mock<ConfigurationDbContext>();

            //List<Client> clients = new List<Client>
            //{
            //    new Client
            //    {
            //        ClientId = "client_id_mvc",
            //        ClientName = "Client MVC",
            //        RequireConsent = false,
            //        AccessTokenLifetime = 3600,
            //        RedirectUris = new List<ClientRedirectUri>
            //        {
            //            new ClientRedirectUri {Id = 1,RedirectUri="Test Url"}
            //        }
            //    }
            //};

            //dbClientContext.Setup(c => c.Clients).Returns(()=> clients);

        }
    }
}
