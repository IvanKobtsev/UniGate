using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs;
using UniGateAPI.DTOs.Response;

namespace UniGateAPI.Controllers;

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
}