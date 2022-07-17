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
    //class to handle routes for ReportingStructures
    [ApiController]
    [Route("api/reportingstructure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        //consistent constructor formatting, needs logger and ReportingStructureService references
        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }
        
        /// <summary>
        /// Controller method to retrieve a ReportingStructure for the requested Employee ID
        /// </summary>
        /// <param name="id">Employee ID from request</param>
        /// <returns></returns>
        [HttpGet("{id}", Name ="getReportingStructureById")]
        public IActionResult GetReportingStructureById(string id)
        {
            //log request
            _logger.LogDebug($"Recieved a repoting structure get request for '{id}'");

            //create reporting structure
            var reportingStructure = _reportingStructureService.createReportingStructure(id);

            //check null
            if (reportingStructure == null)
            {
                return NotFound();
            }

            return Ok(reportingStructure);
        }
    }
}
