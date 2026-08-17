using BPM.Web.API.CustomFilters;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [BPMAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorController : BaseController
    {
        private readonly IDistributorService _distributorService;
        private readonly ILogger<DistributorController> _logger;
        public DistributorController(IDistributorService distributorService, ILogger<DistributorController> logger)
        {
            _distributorService = distributorService;
            _logger = logger;
        }
        [HttpGet]
        [Route("GetDistributorById/{distributorId}")]
        public async Task<IActionResult> GetDistributorById(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching distributor by Id");
                var distributor = await _distributorService.GetDistributorByIdAsync(distributorId);
                if (distributor == null)
                {
                    return BadRequest("unable to fetch Distributor");
                }
                return Ok(distributor);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occcured while fetching distributor with distributorId{distributorId}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("GetAllDistributors")]
        public async Task<IActionResult> GetAllDistributors()
        {
            try
            {
                _logger.LogInformation("fetching AllDistributors");
                var distributors = await _distributorService.GetDistributorListAsync();
                if (distributors == null)
                {
                    _logger.LogError("no records found about distributors");
                    return null;
                }
                return Ok(distributors);
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while fetching distributors");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("onboardDistributor")]
        public async Task<IActionResult> InsertDistributorAsync(CreateDistributorDto distributorDto)
        {
            try
            {
                _logger.LogInformation("Creating Distributor");
                var newDistributor = await _distributorService.InsertDistributorAsync(distributorDto);
                if (!newDistributor)
                {
                    return BadRequest("Unable to create Distributor");
                }
                return Ok(newDistributor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Distributor: {Message}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("updateDistributor/{disributorId}")]
        public async Task<IActionResult> UpdateDistributorAsync(Guid disributorId, UpdateDistributorDto updateDistributorDto)
        {
            try
            {
                _logger.LogInformation("updating Distributor");
                var result = await _distributorService.UpdateDistributorAsync(disributorId, updateDistributorDto);
                if (result != null)
                {
                    return Ok(new { data = result, message = "distributor information updated successfully" });
                }
                return BadRequest("Route Id and DistributorId not matched");
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while updating distributor with distributorId{disributorId}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }
        [HttpDelete]
        [Route("DeleteDistributor/{disributorId}")]
        public async Task<IActionResult> DeleteDistributorAsync(Guid disributorId)
        {
            try
            {
                _logger.LogInformation("Deleting disributor with {DistributorId}:", disributorId);
                var result = await _distributorService.DeleteDistributorById(disributorId);
                if (result != null)
                {
                    return Ok(new { data = result, message = "distributor deleted successfully" });
                   
                }
                return null;
            }
            catch (Exception ex)
            {

                _logger.LogError("error occured while deleting distributor with distributorId{disributorId}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

       
    }
}
