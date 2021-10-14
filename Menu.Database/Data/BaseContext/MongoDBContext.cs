using Common.Mongo.Database.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Common.Mongo.Database.Data.BaseContext
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly MongoDatabaseConfiguration _config;
        private MongoClient client;
        private IMongoDatabase database;
        public MongoDBContext(IOptions<MongoDatabaseConfiguration> options)
        {
            _config = options.Value;
            client = new MongoClient(_config.ConnectionString);
            database = client.GetDatabase(_config.DatabaseName);
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return database.GetCollection<T>(name);
        }
    }
}
