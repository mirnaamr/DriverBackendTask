using DriverBackendTask.Interfaces;
using DriverBackendTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DriverBackendTask.Controllers
{
    [Route("api/Driver")]
    [ApiController]
    public class DriverController : ControllerBase
    {

        private readonly IDriver _driver;
        private readonly ILogger<DriverController> _logger;
        public DriverController(IDriver driver, ILogger<DriverController> logger)
        {
            _driver = driver;
            _logger = logger;
        }

        /// <summary>
        /// Get all drivers
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet("GetAllDrivers")]
        [Authorize]
        public IActionResult GetAllDrivers()
        {
            List<DriverModel> allDrivers = _driver.GetAllDrivers();
            if (allDrivers == null)
            {
                return NotFound(new { message = "No drivers found in the database" });
            }
            return Ok(allDrivers);

        }

        /// <summary>
        /// Add Driver 
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("AddDriver")]
        [Authorize]
        public IActionResult AddDriver([FromBody] DriverModel driverModel)
        {
            int addedDriverId = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                addedDriverId = _driver.AddDriver(driverModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Adding driver: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
            driverModel.Id = addedDriverId;
            return CreatedAtAction(nameof(GetDriver), new { id = driverModel.Id }, driverModel);
        }

        /// <summary>
        /// Get driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns>IActionResult</returns>
        [HttpGet("GetDriver")]
        [Authorize]
        public IActionResult GetDriver(int driverId)
        {
            DriverModel driverModel = _driver.GetDriverById(driverId);
           
            if (driverModel == null)
            {
                return NotFound();
            }

            return Ok(driverModel);
        }


        /// <summary>
        /// Updates driver
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns>IActionResult</returns>
        [HttpPut("UpdateDriver")]
        [Authorize]
        public IActionResult UpdateDriver([FromBody] DriverModel driverModel)
        {
            if (!ModelState.IsValid || driverModel.Id <=0)
            {
                return BadRequest(ModelState);
            }

            if (!_driver.IsDriverExists(driverModel.Id))
            {
                return NotFound(new { message = "Driver not found." });
            }

            try
            {
                _driver.UpdateDriver(driverModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating driver: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }

            return Ok("Driver with id: " + driverModel.Id + " is updated successfully");
        }

        /// <summary>
        /// Deletes driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns>IActionResult</returns>
        [HttpDelete("DeleteDriver")]
        [Authorize]
        public IActionResult DeleteDriver(int driverId)
        {
            if(driverId == 0)
            {
                return BadRequest(new { message = "Invalid driver id" });
            }

            if (!_driver.IsDriverExists(driverId))
            {
                return NotFound(new { message = "Driver not found." });
            }
            try
            {
                _driver.DeleteDriver(driverId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting driver: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }

            return Ok("Driver with id: "+ driverId +" is deleted successfully");
        }

        /// <summary>
        /// Inserts random driver names
        /// </summary>
        /// <param name="driversCountToBeInserted"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("InsertRandomDriverNames")]
        [Authorize]
        public IActionResult InsertRandomDriverNames(int driversCountToBeInserted = 10)
        {
           List<DriverModel> randomDrivers = _driver.InsertRandomDriverNames(driversCountToBeInserted);
            return Ok(randomDrivers);
        }

        /// <summary>
        /// Gets all drivers alphabetized
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet("GetAllDriversAlphabetized")]
        [Authorize]
        public IActionResult GetAllDriversAlphabetized()
        {
            List<AlphabetizedDriverModel> allDriverNamesAlphabetized = _driver.GetAllDriversAlphabetized();
            return Ok(allDriverNamesAlphabetized);
        }

        /// <summary>
        /// Gets alphabetized driver name
        /// </summary>
        /// <param name="driverFullName"></param>
        /// <returns>IActionResult</returns>
        [HttpGet("GetAlphabetizedDriverName")]
        [Authorize]
        public IActionResult GetAlphabetizedDriverName(string driverFullName)
        {
            string alphabetizedDriverName = _driver.GetAlphabetizedDriverName(driverFullName);
            return Ok(alphabetizedDriverName);
        }
    }
}
