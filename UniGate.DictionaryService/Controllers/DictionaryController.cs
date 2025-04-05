using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Controllers;

[ApiController]
[Route("api")]
public class DictionaryController(IImportService importService, IDictionaryService dictionaryService) : ControllerBase
{
    [HttpGet("education_levels")]
    [SwaggerOperation(Summary = "Get all education levels")]
    public async Task<List<EducationLevelDto>> GetLevels()
    {
        return await dictionaryService.GetEducationLevels();
    }

    [HttpGet("document_types")]
    [SwaggerOperation(Summary = "Get all document types")]
    public async Task<List<EducationDocumentTypeDto>> GetDocumentTypes()
    {
        return await dictionaryService.GetEducationDocumentTypes();
    }

    [HttpGet("programs")]
    [SwaggerOperation(Summary = "Get programs")]
    public async Task<EducationProgramsDto> GetPrograms(
        [Range(1, 2147483647)] [FromQuery] int currentPage = 1, [Range(1, 2147483647)] [FromQuery] int pageSize = 10,
        [FromQuery] Guid? facultyId = null, [FromQuery] Guid? educationLevelId = null,
        [FromQuery] string? educationForm = null, [FromQuery] string? language = null,
        [FromQuery] string? programSearch = null)
    {
        return await dictionaryService.GetEducationPrograms(currentPage, pageSize, facultyId, educationLevelId,
            educationForm, language, programSearch);
    }

    [HttpGet("faculties")]
    [SwaggerOperation(Summary = "Get all faculties")]
    public async Task<List<FacultyDto>> GetFaculties()
    {
        return await dictionaryService.GetFaculties();
    }
}