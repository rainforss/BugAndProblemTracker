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
        private readonly ErrorService _errorService;

        public FrameworkController(FrameworkService frameworkService,ErrorService errorService)
        {
            _frameworkService = frameworkService;

            _errorService = errorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguageFrameworksAsync(string languageId)
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

                var results = await _frameworkService.GetLanguageFrameworksAsync(languageId);

                return Ok(results.ToList());
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }



        }

        [HttpGet("{frameworkId}")]
        public async Task<IActionResult> GetFrameworkByIdAsync(string languageId,string frameworkId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }
            if (frameworkId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Framework Id should be a 24 characters hex string" } });
            }

            try
            {
                var errors = await _errorService.GetUriErrors(languageId, frameworkId);

                if (errors.Count != 0)
                {
                    return NotFound(errors);
                }

                var result = await _frameworkService.GetFrameworkByIdAsync(frameworkId);

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }

            
        }


        [HttpPost]
        public async Task<IActionResult> AddFrameworkAsync([FromBody]Framework framework,string languageId)
        {
            if (languageId.Length != 24)
            {
                return BadRequest(new { error = new { message = $"Language Id should be a 24 characters hex string" } });
            }

            if (framework.LanguageId != languageId)
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

                await _frameworkService.AddFrameworkAsync(framework);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new { message = exception.Message } });
            }



            return framework.Id != null ? (IActionResult)Ok(framework.Id) : BadRequest();
        }
    }
}
