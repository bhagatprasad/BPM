using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPM.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugFormController : ControllerBase
    {
        private readonly IDrugFormService _formService;
        private readonly ILogger<DrugFormController> _logger;

        public DrugFormController(
            IDrugFormService formService,
            ILogger<DrugFormController> logger)
        {
            _formService = formService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Drug Forms");

                var data = await _formService.GetAllAsync();

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Forms");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                _logger.LogInformation("Fetching active Drug Forms");

                var data = await _formService.GetActiveFormsAsync();

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active Drug Forms");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("{formId:guid}")]
        public async Task<IActionResult> GetById(Guid formId)
        {
            try
            {
                _logger.LogInformation("Fetching Drug Form with Id {FormId}", formId);

                var data = await _formService.GetByIdAsync(formId);

                if (data == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug Form not found."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Form with Id {FormId}", formId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("code/{formCode}")]
        public async Task<IActionResult> GetByCode(string formCode)
        {
            try
            {
                _logger.LogInformation("Fetching Drug Form with Code {FormCode}", formCode);

                var data = await _formService.GetByCodeAsync(formCode);

                if (data == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug Form not found."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Form with Code {FormCode}", formCode);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("name/{formName}")]
        public async Task<IActionResult> GetByName(string formName)
        {
            try
            {
                _logger.LogInformation("Fetching Drug Form with Name {FormName}", formName);

                var data = await _formService.GetByNameAsync(formName);

                if (data == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug Form not found."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Form with Name {FormName}", formName);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("type/{formType}")]
        public async Task<IActionResult> GetByType(string formType)
        {
            try
            {
                _logger.LogInformation("Fetching Drug Forms with Type {FormType}", formType);

                var data = await _formService.GetByTypeAsync(formType);

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Drug Forms with Type {FormType}", formType);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetFormTypes()
        {
            try
            {
                _logger.LogInformation("Fetching all form types");

                var data = await _formService.GetFormTypesAsync();

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Total = data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching form types");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFiltered([FromBody] DrugFormFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Fetching filtered Drug Forms");

                var (items, totalCount) = await _formService.GetFilteredAsync(filter);

                return Ok(new
                {
                    Success = true,
                    Data = items,
                    Total = totalCount,
                    Page = filter.Page ?? 1,
                    PageSize = filter.PageSize ?? 10
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching filtered Drug Forms");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrugFormDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid data provided.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                _logger.LogInformation("Creating Drug Form");

                var result = await _formService.CreateAsync(dto);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return CreatedAtAction(nameof(GetById), new { formId = result.Data?.FormId }, new
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Drug Form");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] DrugFormBulkCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid data provided.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                if (dto.Forms == null || !dto.Forms.Any())
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "No forms provided for bulk creation."
                    });
                }

                _logger.LogInformation("Creating multiple Drug Forms");

                var result = await _formService.CreateBulkAsync(dto.Forms);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.Data,
                    Total = result.Data.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating multiple Drug Forms");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDrugFormDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid data provided.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                _logger.LogInformation("Updating Drug Form with Id {FormId}", dto.FormId);

                var result = await _formService.UpdateAsync(dto);

                if (!result.Success)
                {
                    return result.Message.Contains("not found")
                        ? NotFound(new { Success = false, Message = result.Message })
                        : BadRequest(new { Success = false, Message = result.Message });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Drug Form with Id {FormId}", dto.FormId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPatch("{formId:guid}")]
        public async Task<IActionResult> Patch(Guid formId, [FromBody] UpdateDrugFormDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid data provided.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                _logger.LogInformation("Patching Drug Form with Id {FormId}", formId);

                // Get existing
                var existing = await _formService.GetByIdAsync(formId);
                if (existing == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Drug Form not found."
                    });
                }

                // Update only provided fields
                var updateDto = new UpdateDrugFormDto
                {
                    FormId = formId,
                    FormCode = !string.IsNullOrEmpty(dto.FormCode) ? dto.FormCode : existing.FormCode,
                    FormName = !string.IsNullOrEmpty(dto.FormName) ? dto.FormName : existing.FormName,
                    FormType = dto.FormType ?? existing.FormType,
                    IsActive = dto.IsActive
                };

                var result = await _formService.UpdateAsync(updateDto);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while patching Drug Form with Id {FormId}", formId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpDelete("{formId:guid}")]
        public async Task<IActionResult> Delete(Guid formId)
        {
            try
            {
                _logger.LogInformation("Deleting Drug Form with Id {FormId}", formId);

                var result = await _formService.DeleteAsync(formId);

                if (!result.Success)
                {
                    if (result.Message.Contains("not found"))
                        return NotFound(new { Success = false, Message = result.Message });

                    if (result.Message.Contains("associated drugs"))
                        return BadRequest(new { Success = false, Message = result.Message });

                    return BadRequest(new { Success = false, Message = result.Message });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Drug Form with Id {FormId}", formId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPatch("{formId:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid formId)
        {
            try
            {
                _logger.LogInformation("Deactivating Drug Form with Id {FormId}", formId);

                var result = await _formService.SoftDeleteAsync(formId);

                if (!result.Success)
                {
                    return result.Message.Contains("not found")
                        ? NotFound(new { Success = false, Message = result.Message })
                        : BadRequest(new { Success = false, Message = result.Message });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deactivating Drug Form with Id {FormId}", formId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpPatch("{formId:guid}/activate")]
        public async Task<IActionResult> Activate(Guid formId)
        {
            try
            {
                _logger.LogInformation("Activating Drug Form with Id {FormId}", formId);

                var result = await _formService.ActivateAsync(formId);

                if (!result.Success)
                {
                    return result.Message.Contains("not found")
                        ? NotFound(new { Success = false, Message = result.Message })
                        : BadRequest(new { Success = false, Message = result.Message });
                }

                return Ok(new
                {
                    Success = true,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while activating Drug Form with Id {FormId}", formId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("validation/form-code/{formCode}")]
        public async Task<IActionResult> ValidateFormCode(string formCode, [FromQuery] Guid? excludeId = null)
        {
            try
            {
                var isValid = await _formService.ValidateFormCodeAsync(formCode, excludeId);

                return Ok(new
                {
                    Success = true,
                    IsValid = isValid,
                    Message = isValid ? "Form code is available." : "Form code already exists."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating form code");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("validation/form-name/{formName}")]
        public async Task<IActionResult> ValidateFormName(string formName, [FromQuery] Guid? excludeId = null)
        {
            try
            {
                var isValid = await _formService.ValidateFormNameAsync(formName, excludeId);

                return Ok(new
                {
                    Success = true,
                    IsValid = isValid,
                    Message = isValid ? "Form name is available." : "Form name already exists."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating form name");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                });
            }
        }
    }
}
