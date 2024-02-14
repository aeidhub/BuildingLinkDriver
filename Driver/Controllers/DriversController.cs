using BuildingLinkDriver.Models;
using Microsoft.AspNetCore.Mvc;
using BuildingLinkDriver.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildingLinkDriver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        public IDriverService _repo { get; set; }
        public DriversController(IDriverService repo)
        {
            _repo = repo;
        }


        // GET: api/Drivers
        [HttpGet]
        public ActionResult<IEnumerable<Models.Driver>> Get()
        {
            try
            {
                var result = _repo.Get();

                if (result.Any())
                    return Ok(result);
                else
                    return NotFound("No drivers found.");
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, $"An error occurred while processing your request. Details: {ex}");
            }
        }

        // GET api/Drivers/5
        [HttpGet("{id}")]
        public ActionResult<Models.Driver> Get(int id)
        {
            try
            {
                var driver = _repo.Get(id);

                if (driver != null)
                    return Ok(driver);
                else
                    return NotFound($"Driver with ID {id} not found.");
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, $"An error occurred while processing your request. Details: {ex}");
            }
        }

        // POST api/Drivers
        [HttpPost]
        public ActionResult Post([FromQuery] Models.Driver driver)
        {
            try
            {
                _repo.Add(driver);
                return CreatedAtAction(nameof(Get), driver);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, $"An error occurred while processing your request. Details: {ex}");
            }
        }

        // PUT api/Drivers/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromQuery] Models.Driver driver)
        {
            try
            {
                var existingDriver = _repo.Get(id);

                if (existingDriver == null)
                    return NotFound($"Driver with ID {id} not found.");

                _repo.Update(driver);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, $"An error occurred while processing your request. Details: {ex}");
            }
        }

        // DELETE api/Drivers/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var existingDriver = _repo.Get(id);

                if (existingDriver == null)
                    return NotFound($"Driver with ID {id} not found.");

                _repo.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, $"An error occurred while processing your request. Details: {ex}");
            }
        }
    }
}
