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

        }

        public IMongoCollection<Bug> Bugs => _db.GetCollection<Bug>("bugs");

        public IMongoCollection<Language> Languages => _db.GetCollection<Language>("languages");

        public IMongoCollection<Framework> Frameworks => _db.GetCollection<Framework>("frameworks");

        public IMongoCollection<Library> Libraries => _db.GetCollection<Library>("libraries");

        public IMongoCollection<Solution> Solutions => _db.GetCollection<Solution>("solutions");

    }
}
