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
    [Produces("application/json")]
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
            try
            {
                var results = await _languageService.GetLanguagesAsync();
                return Ok(results.ToList());
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }
        }

        /// <summary>
        /// Gets a language by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /languages/languageId
        /// 
        /// </remarks>
        /// <param name="languageId"></param>
        /// <returns>A newly created language</returns>
        /// <response code="200">Returns the newly created language</response>
        /// <response code="400">If missing fields</response>

        [HttpGet("{languageId}")]

        public async Task<IActionResult> GetLanguageByIdAsync(string languageId)
        {
            if (languageId.Length != 24)
            {
                ModelState.AddModelError("Language Id", "Language Id must be a 24 character hex string");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _languageService.GetLanguageByIdAsync(languageId);

                if (result == null)
                {
                    return NotFound(new { error = new { message = $"No language with Id {languageId} exists" } });
                }
                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }

        }

        /// <summary>
        /// Creates a language
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /languages
        ///     {
        ///         "name":"language name"
        ///     }
        /// 
        /// </remarks>
        /// <param name="language"></param>
        /// <returns>A newly created language</returns>
        /// <response code="200">Returns the newly created language</response>
        /// <response code="400">If missing fields</response>
        [HttpPost]


        public async Task<IActionResult> AddLanguageAsync([FromBody]Language language)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if(await _languageService.LanguageNameHasDuplicate(language.Name) == true)
                {
                    return BadRequest(new { error = new { message = $"Duplicate language, language with name {language.Name} already exists" } });
                }

                await _languageService.AddLanguageAsync(language);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error=new { message = exception.Message } });
            }



            return language.Id != null ? (IActionResult)Ok(language) : BadRequest();
        }

        //[HttpPut("{languageId}")]

        //public async Task<IActionResult> UpdateLanguageByIdAsync(string languageId,[FromBody] Language updatedLanguage)
        //{
        //    if (languageId.Length != 24)
        //    {
        //        return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if(await _languageService.LanguageExisting(languageId) == false)
        //    {
        //        return NotFound(new { error = new { message = $"Language with Id {languageId} does not exist" } });
        //    }

        //    var result = await _languageService.UpdateLanguageByIdAsync(languageId, updatedLanguage);

        //    return Ok(result);
        //}


    }
}
