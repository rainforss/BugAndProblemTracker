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

        public ErrorService(FrameworkService frameworkService, LanguageService languageService)
        {
            _frameworkService = frameworkService;

            _languageService = languageService;
        }

        public async Task<List<Error>> GetUriErrors(string languageId,string frameworkId)
        {
            List<Task<Error>> errorTasks = new List<Task<Error>>();

            List<Error> errorList = new List<Error>();

            var languageError = _languageService.LanguageValidate(languageId);

            var frameworkError = _frameworkService.FrameworkValidate(languageId, frameworkId);

            errorTasks.Add(languageError);

            errorTasks.Add(frameworkError);

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
