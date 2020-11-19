using BugAndProblemTracker.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Services
{
    public class ErrorService
    {
        private readonly FrameworkService _frameworkService;

        private readonly LanguageService _languageService;
        private readonly BugService _bugService;
        private readonly LibraryService _libraryService;

        public ErrorService(FrameworkService frameworkService, LanguageService languageService, BugService bugService, LibraryService libraryService)
        {
            _frameworkService = frameworkService;

            _languageService = languageService;

            _bugService = bugService;

            _libraryService = libraryService;
        }

        public async Task<List<Error>> GetUriErrors(string languageId,string frameworkId=null,string libraryId=null,string bugId=null)
        {
            List<Task<Error>> errorTasks = new List<Task<Error>>();

            List<Error> errorList = new List<Error>();

            var languageError = _languageService.LanguageValidate(languageId);

            errorTasks.Add(languageError);

            if (frameworkId != null)
            {
                var frameworkError = _frameworkService.FrameworkValidate(languageId, frameworkId);

                errorTasks.Add(frameworkError);
            }

            if (libraryId != null)
            {
                var libraryError = _libraryService.LibraryValidate(languageId, libraryId);

                errorTasks.Add(libraryError);
            }

            var errors = await Task.WhenAll(errorTasks);

            foreach(Error error in errors)
            {
                if (error != null)
                {
                    errorList.Add(error);
                }
            }

            return errorList;



            




        }
    }
}
