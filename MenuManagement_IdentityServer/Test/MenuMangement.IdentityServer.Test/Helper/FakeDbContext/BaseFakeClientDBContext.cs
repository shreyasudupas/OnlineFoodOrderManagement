using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace MenuMangement.IdentityServer.Test.Helper.FakeDbContext
{
    public class BaseFakeClientDBContext
    {
        public ConfigurationDbContext dbClientContext;
        public BaseFakeClientDBContext()
        {
            dbClientContext = CreateDatabaseContext();
        }

        public ConfigurationDbContext CreateDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var FakeConfigurationOptionStore = new Mock<ConfigurationStoreOptions>();

            var dbContext = new ConfigurationDbContext(options, FakeConfigurationOptionStore.Object);

            return dbContext;
        }
    }
}
