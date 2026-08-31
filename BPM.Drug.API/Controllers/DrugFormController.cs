using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.Drug.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DrugFormController : BaseController
    {
        private readonly IDrugFormService _service;
        private readonly ILogger<DrugFormController> _logger;

        public DrugFormController(IDrugFormService service, ILogger<DrugFormController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("get-all-drug-forms")]
        public async Task<IActionResult> GetAllDrugForms()
        {
            _logger.LogInformation("Getting all Drug Forms.");
            var result = await _service.GetAllDrugFormsAsync();
            return Ok(result);
        }

        [HttpGet("get-drug-form-by-id/{formId:guid}")]
        public async Task<IActionResult> GetDrugFormById(Guid formId)
        {
            _logger.LogInformation("Getting Drug Form by Id {FormId}.", formId);
            var result = await _service.GetDrugFormByIdAsync(formId);

            if (result == null)
            {
                return NotFound("Drug Form not found.");
            }

            return Ok(result);
        }

        [HttpGet("get-drug-form-by-code/{formCode}")]
        public async Task<IActionResult> GetDrugFormByCode(string formCode)
        {
            _logger.LogInformation("Getting Drug Form by Code {FormCode}.", formCode);
            var result = await _service.GetDrugFormByCodeAsync(formCode);

            if (result == null)
            {
                return NotFound("Drug Form not found.");
            }

            return Ok(result);
        }

        [HttpGet("get-drug-form-by-name/{formName}")]
        public async Task<IActionResult> GetDrugFormByName(string formName)
        {
            _logger.LogInformation("Getting Drug Form by Name {FormName}.", formName);
            var result = await _service.GetDrugFormByNameAsync(formName);

            if (result == null)
            {
                return NotFound("Drug Form not found.");
            }

            return Ok(result);
        }

        [HttpGet("get-drug-forms-by-type/{formType}")]
        public async Task<IActionResult> GetDrugFormsByType(string formType)
        {
            _logger.LogInformation("Getting Drug Forms by Type {FormType}.", formType);
            var result = await _service.GetDrugFormsByTypeAsync(formType);
            return Ok(result);
        }

        [HttpGet("get-active-drug-forms")]
        public async Task<IActionResult> GetActiveDrugForms()
        {
            _logger.LogInformation("Getting active Drug Forms.");
            var result = await _service.GetActiveDrugFormsAsync();
            return Ok(result);
        }

        [HttpGet("get-drug-form-types")]
        public async Task<IActionResult> GetDrugFormTypes()
        {
            _logger.LogInformation("Getting Drug Form Types.");
            var result = await _service.GetDrugFormTypesAsync();
            return Ok(result);
        }

        [HttpPost("filter-drug-forms")]
        public async Task<IActionResult> GetFilteredDrugForms([FromBody] DrugFormDto.DrugFormFilterDto filter)
        {
            _logger.LogInformation("Getting filtered Drug Forms.");
            var result = await _service.GetFilteredDrugFormsAsync(filter);
            return Ok(new { Success = true, Items = result.Items, TotalCount = result.TotalCount });
        }

        [HttpPost("create-drug-form")]
        public async Task<IActionResult> CreateDrugForm([FromBody] DrugFormDto.CreateDrugFormDto dto)
        {
            _logger.LogInformation("Creating Drug Form with Code {FormCode}.", dto.FormCode);
            var result = await _service.CreateDrugFormAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { Success = result.Success, Message = result.Message, Data = result.Data });
        }

        [HttpPost("create-bulk-drug-forms")]
        public async Task<IActionResult> CreateBulkDrugForms([FromBody] DrugFormDto.DrugFormBulkCreateDto dto)
        {
            _logger.LogInformation("Creating Drug Forms in bulk.");
            var result = await _service.CreateBulkDrugFormsAsync(dto.Forms);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { Success = result.Success, Message = result.Message, Data = result.Data });
        }

        [HttpPut("update-drug-form/{formId:guid}")]
        public async Task<IActionResult> UpdateDrugForm(Guid formId, [FromBody] DrugFormDto.UpdateDrugFormDto dto)
        {
            _logger.LogInformation("Updating Drug Form with Id {FormId}.", formId);

            if (formId != dto.FormId)
            {
                return BadRequest("Form Id mismatch.");
            }

            var result = await _service.UpdateDrugFormAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { Success = result.Success, Message = result.Message, Data = result.Data });
        }

        [HttpPatch("activate-drug-form/{formId:guid}")]
        public async Task<IActionResult> ActivateDrugForm(Guid formId)
        {
            _logger.LogInformation("Activating Drug Form with Id {FormId}.", formId);
            var result = await _service.ActivateDrugFormAsync(formId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { Success = result.Success, Message = result.Message });
        }

        [HttpPatch("deactivate-drug-form/{formId:guid}")]
        public async Task<IActionResult> DeactivateDrugForm(Guid formId)
        {
            _logger.LogInformation("Deactivating Drug Form with Id {FormId}.", formId);
            var result = await _service.SoftDeleteDrugFormAsync(formId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { Success = result.Success, Message = result.Message });
        }

        [HttpGet("validate-drug-form-code/{formCode}")]
        public async Task<IActionResult> ValidateDrugFormCode(string formCode, [FromQuery] Guid? excludeId = null)
        {
            _logger.LogInformation("Validating Drug Form Code {FormCode}.", formCode);
            var result = await _service.ValidateDrugFormCodeAsync(formCode, excludeId);
            return Ok(result);
        }

        [HttpGet("validate-drug-form-name/{formName}")]
        public async Task<IActionResult> ValidateDrugFormName(string formName, [FromQuery] Guid? excludeId = null)
        {
            _logger.LogInformation("Validating Drug Form Name {FormName}.", formName);
            var result = await _service.ValidateDrugFormNameAsync(formName, excludeId);
            return Ok(result);
        }
    }
}