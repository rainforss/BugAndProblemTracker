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

        public LibraryController(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguageLibrariesAsync(string languageId)
        {
            var results = await _libraryService.GetLanguageLibrariesAsync(languageId);

            return Ok(results.ToList());
        }

        [HttpGet("{libraryId}")]

        public async Task<IActionResult> GetLibraryByIdAsync(string libraryId)
        {
            var result = await _libraryService.GetLibraryByIdAsync(libraryId);

            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> AddLibraryAsync([FromBody]Library library, string languageId)
        {
            if (library.Name == null || library.LanguageId == null)
            {
                return BadRequest(new { message = "Library name or language Id cannot be blank" });
            }

            if (library.LanguageId != languageId)
            {
                return BadRequest(new { message = "Language Id does not match an existing language" });
            }

            try
            {
                await _libraryService.AddLibraryAsync(library);
            }
            catch (MongoException mongoException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = mongoException.Message });
            }



            return library.Id != null ? (IActionResult)Ok(library.Id) : BadRequest();
        }
    }
}
