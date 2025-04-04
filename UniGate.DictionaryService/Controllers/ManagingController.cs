using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Controllers;

[ApiController]
[Route("api")]
public class ManagingController(IImportService importService, IDictionaryService dictionaryService) : ControllerBase
{
    // [HttpGet("dictionary/status")]
    // [SwaggerOperation(Summary = "Get dictionary import's status")]
    // public Task<IActionResult> GetImportStatus()
    // {
    //     throw new NotImplementedException();
    // }

    [HttpPost("dictionary/import")]
    [SwaggerOperation(Summary = "Import dictionary")]
    public IActionResult ImportDictionary()
    {
        importService.Import();

        return Ok();
    }
}