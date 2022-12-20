using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using MenuManagement_IdentityServer.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace MenuMangement.IdentityServer.Test.Helper.FakeDbContext
{
    public class BaseFakeClientDBContext
    {
        public ConfigurationDbContext dbClientContext;
        public ApplicationDbContext appContext;
        public BaseFakeClientDBContext()
        {
            dbClientContext = CreateIdentityServerDatabaseContext();
            appContext = CreateApplicationContextDatabaseContext();
        }

        private ConfigurationDbContext CreateIdentityServerDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var FakeConfigurationOptionStore = new Mock<ConfigurationStoreOptions>();

            var dbContext = new ConfigurationDbContext(options, FakeConfigurationOptionStore.Object);

            return dbContext;
        }
        private ApplicationDbContext CreateApplicationContextDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }
    }
}
