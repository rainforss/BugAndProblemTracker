using BugAndProblemTracker.API.Contexts;
using BugAndProblemTracker.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Services
{
    public class SolutionService
    {
        private MongoDBContext _db;

        public SolutionService(MongoDBContext db)
        {
            _db = db;
        }
        public async Task<IAsyncCursor<Solution>> GetSolutionsAsync()
        {
            return await _db.Solutions.FindAsync(s => true);
        }
        public async Task AddSolutionAsync(Solution solution)
        {
            await _db.Solutions.InsertOneAsync(solution);
        }
    }
}
