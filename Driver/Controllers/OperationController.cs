using BuildingLinkDriver.Interfaces;
using BuildingLinkDriver.Models;
using BuildingLinkDriver.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingLinkDriver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        public readonly IOperationService _operationService;
        public readonly IDriverService _driverService;
        private readonly ILogger<OperationController> _logger;

        public OperationController(IOperationService operationService, IDriverService driverService, ILogger<OperationController> logger)
        {
            _operationService = operationService;
            _driverService = driverService;
            _logger = logger;
        }

        [HttpPost]
        [Route("GenerateDrivers/{count}")]
        public IActionResult GenerateDrivers(int count)
        {
            try
            {
                _operationService.CreateRandomDrivers(count);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Failed to generate {Count} drivers.", count);
                return StatusCode(500, $"An error occurred while generating drivers. Details: {ex}");
            }
        }

        [HttpGet]
        [Route("GetAlphabetized")]
        public IActionResult GetAlphabetized()
        {
            try
            {
                var result = _operationService.Alphabetize();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Failed to retrieve and alphabetize drivers.");
                return StatusCode(500, $"An error occurred while retrieving alphabetized drivers. Details: {ex}");
            }
        }

        [HttpGet]
        [Route("GetAlphabetized/{id}")]
        public IActionResult GetAlphabetized(int id)
        {
            try
            {
                Driver? driver = _driverService.Get(id);
                if (driver is null)
                    return NotFound("Driver not found.");

                var result = _operationService.Alphabetize(driver);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Failed to retrieve and alphabetize driver with ID {Id}.", id);
                return StatusCode(500, $"An error occurred while retrieving the driver with ID {id}. Details: {ex}");
            }
        }
    }
}
