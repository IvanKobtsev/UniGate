using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.DictionaryService.DTOs.Response;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Controllers;

[ApiController]
[Route("api")]
public class DictionaryController(IImportService importService, IDictionaryService dictionaryService) : ControllerBase
{
    [HttpGet("programs")]
    [SwaggerOperation(Summary = "Get all education programs")]
    public async Task<List<EducationLevelDto>> GetLevels()
    {
        return await dictionaryService.GetEducationLevels();
    }
}