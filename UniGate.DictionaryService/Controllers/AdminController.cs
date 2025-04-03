using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.DictionaryService.DTOs.Common;
using UniGate.DictionaryService.DTOs.Response;

namespace UniGate.DictionaryService.Controllers;

[ApiController]
[Route("api")]
public class AdminController : ControllerBase
{
    [HttpGet("dictionary/status")]
    [SwaggerOperation(Summary = "Get dictionary import's status")]
    public ImportStatusDto GetImportStatus()
    {
        throw new NotImplementedException();
    }

    [HttpPost("dictionary/import")]
    [SwaggerOperation(Summary = "Import dictionary")]
    public void ImportDictionary()
    {
        throw new NotImplementedException();
    }

    [HttpGet("programs")]
    [SwaggerOperation(Summary = "Get all education programs")]
    public List<EducationProgramDto> GetPrograms()
    {
        throw new NotImplementedException();
    }
}