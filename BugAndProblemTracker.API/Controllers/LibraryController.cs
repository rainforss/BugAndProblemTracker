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
    [Route("api/languages/{languageId}/libraries")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _libraryService;
        private readonly ErrorService _errorService;

        public LibraryController(LibraryService libraryService, ErrorService errorService)
        {
            _libraryService = libraryService;

            _errorService = errorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguageLibrariesAsync(string languageId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }

            try
            {
                var errors = await _errorService.GetUriErrors(languageId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                var results = await _libraryService.GetLanguageLibrariesAsync(languageId);

                return Ok(results.ToList());
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }

            
        }

        [HttpGet("{libraryId}")]

        public async Task<IActionResult> GetLibraryByIdAsync(string languageId,string libraryId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }
            if (libraryId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Framework Id should be a 24 characters hex string" } });
            }

            try
            {
                var errors = await _errorService.GetUriErrors(languageId, libraryId: libraryId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                var result = await _libraryService.GetLibraryByIdAsync(libraryId);

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }


        }

        [HttpPost]

        public async Task<IActionResult> AddLibraryAsync([FromBody]Library library, string languageId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }

            if (library.LanguageId != languageId)
            {
                ModelState.AddModelError("Language unmatch", "Framework Id does not match an existing framework");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var errors = await _errorService.GetUriErrors(languageId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                await _libraryService.AddLibraryAsync(library);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }



            return library.Id != null ? (IActionResult)Ok(library.Id) : BadRequest();
        }
    }
}
