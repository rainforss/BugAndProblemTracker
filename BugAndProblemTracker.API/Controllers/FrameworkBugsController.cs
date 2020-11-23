using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BugAndProblemTracker.API.Models;
using BugAndProblemTracker.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BugAndProblemTracker.API.Controllers
{
    [ApiController]
    [Route("api/languages/{languageId}/frameworks/{frameworkId}/bugs")]
    public class FrameworkBugsController : ControllerBase
    {
        private readonly IBugService _bugService;
        private readonly FrameworkService _frameworkService;
        private readonly ErrorService _errorService;

        public FrameworkBugsController(IBugService bugService,FrameworkService frameworkService,ErrorService errorService)
        {
            _bugService = bugService;
            _frameworkService = frameworkService;
            _errorService = errorService;
        }




        [HttpGet]

        public async Task<IActionResult> GetFrameworkBugsAsync(string frameworkId, string languageId)
        {
            if (frameworkId.Length != 24)
            {
                ModelState.AddModelError("Route", "Framework Id in URI must be 24 characters hex string");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var errors = await _errorService.GetUriErrors(languageId, frameworkId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                var results = await _bugService.GetFrameworkBugsAsync(frameworkId);

                return Ok(results.ToList());
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }
            

            
        }



        [HttpGet("{bugId}")]

        public async Task<IActionResult> GetBugByIdAsync(string bugId,string frameworkId,string languageId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }
            if (frameworkId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Framework Id should be a 24 characters hex string" } });
            }
            if (bugId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Bug Id should be a 24 characters hex string" } });
            }

            try
            {
                var errors = await _errorService.GetUriErrors(languageId, frameworkId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                var result = await _bugService.GetBugByIdAsync(bugId);

                if (result == null)
                {
                    return NotFound(new { error = new { message = $"No bug with Id {bugId} exists" } });
                }

                return Ok(result);
            }
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }

            


        }

        [HttpPost]
        public async Task<IActionResult> PostBugAsync([FromBody]Bug bug,string frameworkId, string languageId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }
            if (frameworkId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Framework Id should be a 24 characters hex string" } });
            }

            if (bug.LanguageId != languageId)
            {
                ModelState.AddModelError("Language unmatch", "Framework Id does not match an existing framework");
            }

            if (bug.FrameworkId != frameworkId)
            {
                ModelState.AddModelError("Framework unmatch", "Framework Id does not match an existing framework");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                       
            try
            {
                var errors = await _errorService.GetUriErrors(languageId, frameworkId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                await _bugService.AddBugAsync(bug);
            }
            catch (Exception exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }

            

            return bug.Id != null ? (IActionResult) Ok(bug.Id) : BadRequest();
        }

        [HttpDelete("{bugId}")]
        public async Task<IActionResult> DeleteBugByIdAsync(string languageId,string frameworkId,string bugId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }
            if (frameworkId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Framework Id should be a 24 characters hex string" } });
            }
            if (bugId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Bug Id should be a 24 characters hex string" } });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var errors = await _errorService.GetUriErrors(languageId, frameworkId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                var toBeDeleted = await _bugService.GetBugByIdAsync(bugId);

                if (toBeDeleted == null)
                {
                    return NotFound(new { error = new { message = $"No bug with Id {bugId} exists" } });
                }

                var result= await _bugService.DeleteFrameworkBugByIdAsync(frameworkId, bugId);

                return Ok(result);
            }
            catch(MongoException mongoException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = mongoException.Message });
            }
        }

        [HttpPut("{bugId}")]
        public async Task<IActionResult> UpdateBug(string bugId, string languageId, string frameworkId, [FromBody]Bug updatedBug)
        {
            if (updatedBug.FrameworkId.Length != 24)
            {
                ModelState.AddModelError("Framework Id", "Framework Id must be a 24 characters hex string");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try {

                var errors = await _errorService.GetUriErrors(languageId, frameworkId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                var result = await _bugService.UpdateBugByIdAsync(bugId, updatedBug);

                return Ok(result);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        
    }
}
