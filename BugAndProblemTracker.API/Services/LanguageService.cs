using BugAndProblemTracker.API.Contexts;
using BugAndProblemTracker.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Services
{
    public class LanguageService
    {
        private MongoDBContext _db;

        public LanguageService(MongoDBContext db)
        {
            _db = db;
        }
        public async Task<IAsyncCursor<Language>> GetLanguagesAsync()
        {
            var results = await _db.Languages.FindAsync(l => true);
            return results;
        }

        public async Task<Language> GetLanguageByIdAsync(string languageId)
        {
            return await _db.Languages.Find(l => l.Id == languageId).SingleOrDefaultAsync();
        }

        public async Task<bool> LanguageExisting(string languageId)
        {
            return await _db.Languages.Find(l => l.Id == languageId).SingleOrDefaultAsync() != null;
        }

        public async Task<Error> LanguageValidate(string languageId)
        {
            var result = await _db.Languages.Find(l => l.Id == languageId).SingleOrDefaultAsync();

            if (result == null)
            {
                return new Error() { Id = languageId, Message = "The resource could not be found", Type = "Language Id" };
            }

            return null;
        }

        public async Task<bool> LanguageNameHasDuplicate(string name)
        {
            var result = await _db.Languages.Find(l => l.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();

            if(result == null)
            {
                return false;
            }

            return true;
        }

        public async Task AddLanguageAsync(Language language)
        {
            var keys = Builders<Language>.IndexKeys.Ascending("name");

            var indexOptions = new CreateIndexOptions { Unique = true };

            var model = new CreateIndexModel<Language>(keys, indexOptions);

            _db.Languages.Indexes.CreateOne(model);

            await _db.Languages.InsertOneAsync(language);
        }

        public async Task<Language> UpdateLanguageByIdAsync(string languageId,Language updatedLanguage)
        {
            var options = new FindOneAndReplaceOptions<Language>
            {
                ReturnDocument = ReturnDocument.After
            };

            var filter = Builders<Language>.Filter.Eq("Id", languageId);

            var result = await _db.Languages.FindOneAndReplaceAsync(filter, updatedLanguage, options);

            return result;
        }
    }
}
