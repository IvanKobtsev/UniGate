using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Controllers;

[ApiController]
[Route("api/v1")]
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
    public async Task<EducationProgramsPagedListDto> GetPrograms(
        [Range(1, int.MaxValue)] [FromQuery] int currentPage = 1,
        [Range(1, int.MaxValue)] [FromQuery] int pageSize = 10,
        [FromQuery] Guid? facultyId = null, [Range(0, int.MaxValue)] [FromQuery] int? educationLevelId = null,
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