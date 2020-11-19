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

        public async Task<bool> FrameworkExisting(string languageId,string frameworkId)
        {
            var result = await _db.Frameworks.Find(f => f.Id == frameworkId&&f.LanguageId==languageId).SingleOrDefaultAsync();

            return result != null;
        }

        public async Task<Error> FrameworkValidate(string languageId, string frameworkId)
        {
            var result = await _db.Frameworks.Find(f => f.Id == frameworkId && f.LanguageId == languageId).SingleOrDefaultAsync();

            if (result == null)
            {
                return new Error() { Id = frameworkId, Message = "The resource could not be found", Type = "Framework Id" };
            }

            return null;
        }

        public async Task<bool> FrameworkNameHasDuplicate(string name)
        {
            var result = await _db.Frameworks.Find(f => f.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }

            return true;
        }

        public async Task AddFrameworkAsync(Framework framework)
        {
            var keys = Builders<Framework>.IndexKeys.Ascending("name");

            var indexOptions = new CreateIndexOptions { Unique = true };

            var model = new CreateIndexModel<Framework>(keys, indexOptions);

            _db.Frameworks.Indexes.CreateOne(model);

            await _db.Frameworks.InsertOneAsync(framework);
        }

        public async Task<Framework> UpdateFrameworkByIdAsync(string frameworkId, Framework updatedFramework)
        {
            var options = new FindOneAndReplaceOptions<Framework>
            {
                ReturnDocument = ReturnDocument.After
            };

            var filter = Builders<Framework>.Filter.Eq("Id", frameworkId);

            var result = await _db.Frameworks.FindOneAndReplaceAsync(filter, updatedFramework, options);

            return result;
        }
    }
}
