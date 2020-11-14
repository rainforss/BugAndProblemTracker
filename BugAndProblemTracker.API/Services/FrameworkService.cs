using BugAndProblemTracker.API.Contexts;
using BugAndProblemTracker.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Services
{
    public class FrameworkService
    {
        private MongoDBContext _db;

        public FrameworkService(MongoDBContext db)
        {
            _db = db;
        }
        public async Task<IAsyncCursor<Framework>> GetFrameworksAsync()
        {
            return await _db.Frameworks.FindAsync(f => true);
        }

        public async Task<IAsyncCursor<Framework>> GetLanguageFrameworksAsync(string languageId)
        {
            return await _db.Frameworks.FindAsync(f => f.LanguageId == languageId);
        }

        public async Task<Framework> GetFrameworkByIdAsync(string frameworkId)
        {
            return await _db.Frameworks.Find(f => f.Id == frameworkId).SingleOrDefaultAsync();
        }
        public async Task AddFrameworkAsync(Framework framework)
        {
            var keys = Builders<Framework>.IndexKeys.Ascending("name");

            var indexOptions = new CreateIndexOptions { Unique = true };

            var model = new CreateIndexModel<Framework>(keys, indexOptions);

            _db.Frameworks.Indexes.CreateOne(model);

            await _db.Frameworks.InsertOneAsync(framework);
        }
    }
}
