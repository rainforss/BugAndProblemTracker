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
    [Route("api/languages/{languageId}/frameworks")]
    [ApiController]
    public class FrameworkController : ControllerBase
    {
        private readonly FrameworkService _frameworkService;

        public FrameworkController(FrameworkService frameworkService)
        {
            _frameworkService = frameworkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguageFrameworksAsync(string languageId)
        {
            var results = await _frameworkService.GetLanguageFrameworksAsync(languageId);

            return Ok(results.ToList());

        }

        [HttpGet("{frameworkId}")]
        public async Task<IActionResult> GetFrameworkByIdAsync(string frameworkId)
        {
            var result = await _frameworkService.GetFrameworkByIdAsync(frameworkId);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddFrameworkAsync([FromBody]Framework framework,string languageId)
        {

            if (framework.Name == null || framework.LanguageId ==null)
            {
                return BadRequest(new { message = "Framework name or language Id cannot be blank" });
            }

            if (framework.LanguageId != languageId)
            {
                return BadRequest(new { message = "Language Id does not match an existing language" });
            }

            try
            {
                await _frameworkService.AddFrameworkAsync(framework);
            }
            catch (MongoException mongoException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = mongoException.Message });
            }



            return framework.Id != null ? (IActionResult)Ok(framework.Id) : BadRequest();
        }
    }
}
