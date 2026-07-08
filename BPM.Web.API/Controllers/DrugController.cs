using BPM.Web.API.Models.Entities;
using BPM.Web.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugController : ControllerBase
    {
        private readonly IDrugService _drugService;

        public DrugController(IDrugService drugservice)
        {
            _drugService = drugservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var drugs = await _drugService.GetAllDrugsAsync();
            return Ok(drugs);
        }

        [HttpGet("{drugid}")]
        public async Task<IActionResult> Get(Guid drugid)
        {
            var drug = await _drugService.GetDrugByIdAsync(drugid);

            if (drug == null)
                return NotFound("Drug not found.");

            return Ok(drug);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Drug drug)
        {
            var result = await _drugService.InsertDrugAsync(drug);

            if (!result)
                return BadRequest("Failed to create drug.");

            return Ok("Drug Created Successfully");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Drug drug)
        {
            if (drug == null)
                return BadRequest("Invalid request.");

            var result = await _drugService.UpdateDrugAsync(drug);

            if (!result)
                return NotFound(new
                {
                    Message = "Drug not found."
                });

            return Ok(new
            {
                Message = "Drug updated successfully."
            });
        }

        [HttpDelete("{drugId:guid}")]
        public async Task<IActionResult> Delete(Guid drugId)
        {
            var result = await _drugService.DeleteDrugAsync(drugId);

            if (!result)
                return NotFound(new
                {
                    Message = "Drug not found."
                });

            return Ok(new
            {
                Message = "Drug deleted successfully."
            });
        }
    }
}
