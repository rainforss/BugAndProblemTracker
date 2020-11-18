using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugAndProblemTracker.API.Contexts;
using BugAndProblemTracker.API.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BugAndProblemTracker.API.Services
{
    public class BugService:IBugService
    {
        private readonly MongoDBContext _db;

        public BugService(MongoDBContext db)
        {
            _db = db;
        }

        public async Task<IAsyncCursor<Bug>> GetFrameworkBugsAsync(string frameworkId)
        {
            var results = await _db.Bugs.FindAsync(b => b.FrameworkId==frameworkId);

            return results;
        }

        public async Task<IAsyncCursor<Bug>> GetLibraryBugsAsync(string libraryId)
        {
            var results = await _db.Bugs.FindAsync(b => b.LibraryId == libraryId);

            return results;
        }

        public async Task<IAsyncCursor<Bug>> GetBugsAsync()
        {
            var results = await _db.Bugs.FindAsync(b => true);

            return results;
        }


        public async Task<Bug> GetBugByIdAsync(string bugId)
        {
            return await _db.Bugs.Find(b => b.Id == bugId).SingleOrDefaultAsync();
        }

        public async Task<bool> BugExisting(string bugId)
        {
            return await _db.Bugs.Find(b => b.Id == bugId).SingleOrDefaultAsync() != null;
        }

        

        public async Task AddBugAsync(Bug bug)
        {
            var keys = Builders<Bug>.IndexKeys.Ascending("Name");

            var indexOptions = new CreateIndexOptions { Unique = true };

            var model = new CreateIndexModel<Bug>(keys, indexOptions);

            _db.Bugs.Indexes.CreateOne(model);

            await _db.Bugs.InsertOneAsync(bug);
        }

        public async Task<Bug> DeleteFrameworkBugByIdAsync(string frameworkId, string bugId)
        {
            var deleteResult = await _db.Bugs.FindOneAndDeleteAsync(b => b.Id == bugId&&b.FrameworkId==frameworkId);

            return deleteResult;
        }

        public async Task<Bug> UpdateBugByIdAsync(string bugId, Bug updatedBug)
        {


            var options = new FindOneAndReplaceOptions<Bug>
            {
                ReturnDocument = ReturnDocument.After
            };

            var filter = Builders<Bug>.Filter.Eq("Id", bugId);

            var result = await _db.Bugs.FindOneAndReplaceAsync(filter, updatedBug,options);

            return result;
        }

    }
}
