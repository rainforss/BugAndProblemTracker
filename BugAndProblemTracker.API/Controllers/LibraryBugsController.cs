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
    [Route("api/languages/{languageId}/library/{libraryId}/bugs")]
    [ApiController]
    public class LibraryBugsController : ControllerBase
    {
        private readonly BugService _bugService;

        public LibraryBugsController(BugService bugService)
        {
            _bugService = bugService;
        }




        [HttpGet]

        public async Task<IActionResult> GetLibraryBugsAsync(string libraryId)
        {
            try
            {
                var results = await _bugService.GetFrameworkBugsAsync(libraryId);

                return Ok(results.ToList());
            }
            catch (MongoException mongoException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = mongoException.Message });
            }



        }



        [HttpGet("{bugId}")]

        public async Task<IActionResult> GetBugByIdAsync(string bugId)
        {
            var result = await _bugService.GetBugByIdAsync(bugId);

            return Ok(result);


        }

        [HttpPost]
        public async Task<IActionResult> PostBugAsync([FromBody] Bug bug, string libraryId)
        {

            if (bug.Name == null || bug.Description == null)
            {
                return BadRequest(new { message = "Bug name or description cannot be blank" });
            }

            if (bug.LibraryId != libraryId)
            {
                return BadRequest(new { message = "Library Id does not match an existing library" });
            }

            try
            {
                await _bugService.AddBugAsync(bug);
            }
            catch (MongoException mongoException)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = mongoException.Message });
            }



            return bug.Id != null ? (IActionResult)Ok(bug.Id) : BadRequest();
        }
    }
}
