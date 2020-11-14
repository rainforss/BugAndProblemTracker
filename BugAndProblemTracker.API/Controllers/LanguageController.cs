using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugAndProblemTracker.API.Models;
using BugAndProblemTracker.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BugAndProblemTracker.API.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly LanguageService _languageService;

        public LanguageController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]

        public async Task<IActionResult> GetLanguagesAsync()
        {
            var results = await _languageService.GetLanguagesAsync();
            return Ok(results.ToList());
        }

        [HttpGet("{languageId}")]

        public async Task<IActionResult> GetLanguageByIdAsync(string languageId)
        {
            var result = await _languageService.GetLanguageByIdAsync(languageId);
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> AddLanguageAsync([FromBody]Language language)
        {
            if (language.Name == null)
            {
                return BadRequest(new { message = "Language name cannot be blank" });
            }

            try
            {
                await _languageService.AddLanguageAsync(language);
            }
            catch (MongoException mongoException)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = mongoException.Message });
            }



            return language.Id != null ? (IActionResult)Ok(language.Id) : BadRequest();
        }
    }
}
