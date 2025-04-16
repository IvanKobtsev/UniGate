using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.Common.Filters;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Controllers;

[ApiController]
[Route("api/v1/validation")]
public class ValidationController(IValidationService validationService) : ControllerBase
{
    [RequireHmac]
    [HttpGet("programs/{programToChoose:guid}")]
    [SwaggerOperation(Summary = "Check if user can apply for specified program and get faculty of it")]
    public async Task<IActionResult> ValidateProgramApplication(
        [FromRoute] Guid programToChoose,
        [FromQuery] Guid? alreadyChosenProgram,
        [FromQuery] Guid? educationDocumentTypeId)
    {
        var result = await validationService.ValidateProgramApplicationAndGetFacultyId(programToChoose,
            alreadyChosenProgram,
            educationDocumentTypeId);

        return Ok(result);
    }
}