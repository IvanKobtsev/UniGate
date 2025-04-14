using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs.Common;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api/v1/managers")]
public class ChiefManagerController : ControllerBase
{
    [HttpGet("")]
    [SwaggerOperation(Summary = "Get all managers paginated")]
    public List<ManagerLightDto> GetManagers()
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id:guid}/admissions")]
    [SwaggerOperation(Summary = "Assign an admission to manager")]
    public void AssignManager(Guid id, [FromBody] Guid admissionId)
    {
        throw new NotImplementedException();
    }
}