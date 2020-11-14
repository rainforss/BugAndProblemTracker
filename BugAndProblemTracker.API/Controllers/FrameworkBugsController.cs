using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly BugService _bugService;

        public FrameworkBugsController(BugService bugService)
        {
            _bugService = bugService;
        }




        [HttpGet]

        public async Task<IActionResult> GetFrameworkBugsAsync(string frameworkId)
        {
            try
            {
                var results = await _bugService.GetFrameworkBugsAsync(frameworkId);

                return Ok(results.ToList());
            }
            catch (MongoException mongoException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new {message=mongoException.Message });
            }
            

            
        }



        [HttpGet("{bugId}")]

        public async Task<IActionResult> GetBugByIdAsync(string bugId)
        {
            var result = await _bugService.GetBugByIdAsync(bugId);

            return Ok(result);


        }

        [HttpPost]
        public async Task<IActionResult> PostBugAsync([FromBody]Bug bug,string frameworkId)
        {

            if(bug.Name==null || bug.Description == null)
            {
                return BadRequest(new { message="Bug name or description cannot be blank" });
            }

            if (bug.FrameworkId != frameworkId)
            {
                return BadRequest(new { message = "Framework Id does not match an existing framework" });
            }

            try
            {
                await _bugService.AddBugAsync(bug);
            }
            catch (MongoException mongoException)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = mongoException.Message });
            }

            

            return bug.Id != null ? (IActionResult) Ok(bug.Id) : BadRequest();
        }

        
    }
}
