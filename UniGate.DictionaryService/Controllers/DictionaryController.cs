using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Controllers;

[ApiController]
[Route("api/v1")]
public class DictionaryController(IImportService importService, IDictionaryService dictionaryService) : ControllerBase
{
    [HttpGet("education-levels")]
    [SwaggerOperation(Summary = "Get education levels (paginated)")]
    public async Task<IActionResult> GetLevels([Range(1, int.MaxValue)] [FromQuery] int currentPage = 1,
        [Range(1, int.MaxValue)] [FromQuery] int pageSize = 10, [FromQuery] string? levelName = null)
    {
        return Ok(await dictionaryService.GetEducationLevels(currentPage, pageSize, levelName));
    }

    [HttpGet("document-types")]
    [SwaggerOperation(Summary = "Get education document types (paginated)")]
    public async Task<IActionResult> GetDocumentTypes(
        [Range(1, int.MaxValue)] [FromQuery] int currentPage = 1,
        [Range(1, int.MaxValue)] [FromQuery] int pageSize = 10, [FromQuery] string? documentTypeName = null)
    {
        return Ok(await dictionaryService.GetEducationDocumentTypes(currentPage, pageSize, documentTypeName));
    }

    [HttpGet("programs")]
    [SwaggerOperation(Summary = "Get education programs (paginated)")]
    public async Task<IActionResult> GetPrograms(
        [Range(1, int.MaxValue)] [FromQuery] int currentPage = 1,
        [Range(1, int.MaxValue)] [FromQuery] int pageSize = 10,
        [FromQuery] Guid? facultyId = null, [Range(0, int.MaxValue)] [FromQuery] int? educationLevelId = null,
        [FromQuery] string? educationForm = null, [FromQuery] string? language = null,
        [FromQuery] string? programNameOrCode = null)
    {
        return Ok(await dictionaryService.GetEducationPrograms(currentPage, pageSize, facultyId, educationLevelId,
            educationForm, language, programNameOrCode));
    }

    [HttpGet("faculties")]
    [SwaggerOperation(Summary = "Get faculties (paginated)")]
    public async Task<IActionResult> GetFaculties([Range(1, int.MaxValue)] [FromQuery] int currentPage = 1,
        [Range(1, int.MaxValue)] [FromQuery] int pageSize = 10, [FromQuery] string? facultyName = null)
    {
        return Ok(await dictionaryService.GetFaculties(currentPage, pageSize, facultyName));
    }
}