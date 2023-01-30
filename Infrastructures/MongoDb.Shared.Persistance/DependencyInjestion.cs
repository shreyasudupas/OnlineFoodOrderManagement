using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDb.Shared.Persistance.DBContext;

namespace MongoDb.Shared.Persistance
{
    public static class DependencyInjestion
    {
        public static IServiceCollection AddSharedMongoServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.Configure<MongoDatabaseConfiguration>(configuration.GetSection("MongoOrderDBSettings"));
            return services;
        }
    }
}
