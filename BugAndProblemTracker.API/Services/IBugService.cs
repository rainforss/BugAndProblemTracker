using BugAndProblemTracker.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Services
{
    public interface IBugService
    {
        Task<IAsyncCursor<Bug>> GetBugsAsync();

        Task<IAsyncCursor<Bug>> GetFrameworkBugsAsync(string frameworkId);

        Task<IAsyncCursor<Bug>> GetLibraryBugsAsync(string libraryId);

        Task<Bug> GetBugByIdAsync(string bugId);

        Task AddBugAsync(Bug bug);

        Task<bool> BugExisting(string bugId);

        Task<Bug> DeleteFrameworkBugByIdAsync(string frameworkId, string bugId);

        Task<Bug> UpdateBugByIdAsync(string bugId, Bug updatedBug);

    }
}
