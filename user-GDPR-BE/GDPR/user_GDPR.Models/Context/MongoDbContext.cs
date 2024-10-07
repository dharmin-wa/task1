using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace user_GDPR.Models.Context
{
    public class MongoDbContext
    {

        private readonly IMongoCollection<Users> _userCollection;

        public MongoDbContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<Users>(mongoDBSettings.Value.CollectionName);
        }

        public IMongoCollection<Users> Users => _userCollection;

    }
}