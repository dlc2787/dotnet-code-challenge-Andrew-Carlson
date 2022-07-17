using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        //instance vars
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        //constructor
        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        /// <summary>
        /// A method to create a new entry for a compensation object
        /// </summary>
        /// <remarks>
        ///     potential bug: I'm assuming handling duplicate entries is handled by the context and/or the repository... I'm
        ///     not sure why a new GUID is assigned when a new employee is added, but I didn't do that for this, since the
        ///     employee already exists. Duplicate entries may not be properly handled.
        /// </remarks>
        /// <param name="compensation">The compensation object to add</param>
        /// <returns>an Ok status and the compensation object, or a not found status if null</returns>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            //log the create compensation request
            //_logger.LogDebug($"Received compensation create request for employee '{compensation.employee.FirstName} {compensation.employee.LastName}'");
            _logger.LogDebug($"Received compensation create request for employee '{compensation.employee}'");

            //create compensation
            _compensationService.Create(compensation);

            //create a new get request here to retrieve the added compensation object, and return it in the response using the compensation object as a out variable
            //return CreatedAtRoute("getCompensationById", new { id = compensation.CompensationId }, compensation);
            return CreatedAtRoute("getCompensationById", new { id = compensation.employee }, compensation);
        }

        /// <summary>
        /// A method to retrieve and serve a compensation object from a requested employee ID
        /// </summary>
        /// <param name="id">the ID requested to find a compensation for</param>
        /// <returns>An ok and the compensation object, or a Not Found status if null</returns>
        [HttpGet("{id}", Name ="getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            //log request
            _logger.LogDebug($"Recieved compensation get request for '{id}'");

            //retrieve compensation
            var compensation = _compensationService.GetCompById(id);

            //return not found status code if compensation is null
            if (compensation == null)
            {
                return NotFound();
            }

            //return ok status code and compensation object
            return Ok(compensation);
        }
    }
}
