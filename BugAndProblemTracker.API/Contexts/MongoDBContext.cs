using BugAndProblemTracker.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Contexts
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _db;

        public MongoDBContext(IMongoClient client, string dbName)
        {
            _db = client.GetDatabase(dbName);

            var keys = Builders<Bug>.IndexKeys.Ascending("name");

            var indexOptions = new CreateIndexOptions { Unique = true };

            var model = new CreateIndexModel<Bug>(keys, indexOptions);

            _db.GetCollection<Bug>("bugs").Indexes.CreateOne(model);

        }

        public IMongoCollection<Bug> Bugs => _db.GetCollection<Bug>("bugs");

        public IMongoCollection<Language> Languages => _db.GetCollection<Language>("languages");

        public IMongoCollection<Framework> Frameworks => _db.GetCollection<Framework>("frameworks");

        public IMongoCollection<Library> Libraries => _db.GetCollection<Library>("libraries");

        public IMongoCollection<Solution> Solutions => _db.GetCollection<Solution>("solutions");

    }
}
