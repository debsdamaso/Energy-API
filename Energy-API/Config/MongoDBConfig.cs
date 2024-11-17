using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Energy_API.Config
{
    public class MongoDBConfig
    {
        private readonly IMongoDatabase _database;

        public MongoDBConfig(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["MongoDB:ConnectionString"]);
            _database = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}
