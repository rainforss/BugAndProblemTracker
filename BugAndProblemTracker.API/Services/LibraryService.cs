using BugAndProblemTracker.API.Contexts;
using BugAndProblemTracker.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Services
{
    public class LibraryService
    {
        private MongoDBContext _db;

        public LibraryService(MongoDBContext db)
        {
            _db = db;
        }
        public async Task<IAsyncCursor<Library>> GetLibrariesAsync()
        {
            return await _db.Libraries.FindAsync(l => true);
        }

        public async Task<IAsyncCursor<Library>> GetLanguageLibrariesAsync(string languageId)
        {
            return await _db.Libraries.FindAsync(l => l.LanguageId == languageId);
        }

        public async Task<Library> GetLibraryByIdAsync(string libraryId)
        {
            return await _db.Libraries.Find(l => l.Id == libraryId).SingleOrDefaultAsync();
        }
        public async Task AddLibraryAsync(Library library)
        {
            var keys = Builders<Library>.IndexKeys.Ascending("name");

            var indexOptions = new CreateIndexOptions { Unique = true };

            var model = new CreateIndexModel<Library>(keys, indexOptions);

            _db.Libraries.Indexes.CreateOne(model);

            await _db.Libraries.InsertOneAsync(library);
        }
    }
}
