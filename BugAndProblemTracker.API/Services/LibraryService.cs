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

        public async Task<bool> LibraryExisting(string libraryId)
        {
            return await _db.Libraries.Find(l => l.Id == libraryId).SingleOrDefaultAsync() != null;
        }

        public async Task<Error> LibraryValidate(string languageId,string libraryId)
        {
            var result = await _db.Libraries.Find(l => l.Id == libraryId&&l.LanguageId==languageId).SingleOrDefaultAsync();

            if (result == null)
            {
                return new Error() { Id = libraryId, Message = "The resource could not be found", Type = "Library Id" };
            }

            return null;
        }

        public async Task<bool> LibraryNameHasDuplicate(string name)
        {
            var result = await _db.Libraries.Find(l => l.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }

            return true;
        }

        public async Task AddLibraryAsync(Library library)
        {
            var keys = Builders<Library>.IndexKeys.Ascending("name");

            var indexOptions = new CreateIndexOptions { Unique = true };

            var model = new CreateIndexModel<Library>(keys, indexOptions);

            _db.Libraries.Indexes.CreateOne(model);

            await _db.Libraries.InsertOneAsync(library);
        }

        public async Task<Library> UpdateLibraryByIdAsync(string libraryId,Library updatedLibrary)
        {
            var options = new FindOneAndReplaceOptions<Library>
            {
                ReturnDocument = ReturnDocument.After
            };

            var filter = Builders<Library>.Filter.Eq("Id", libraryId);

            var result = await _db.Libraries.FindOneAndReplaceAsync(filter, updatedLibrary, options);

            return result;
        }
    }
}
