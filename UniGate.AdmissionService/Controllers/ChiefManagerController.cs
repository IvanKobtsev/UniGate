using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api/v1")]
public class ChiefManagerController : ControllerBase
{
    [HttpPost("managers/chiefs")]
    [SwaggerOperation(Summary = "Create a chief manager")]
    public Guid CreateManager([FromBody] CreateChiefManagerDto chiefManagerDto)
    {
        throw new NotImplementedException();
    }

    [HttpGet("managers")]
    [SwaggerOperation(Summary = "Get all managers")]
    public List<ShallowManagerDto> GetManagers()
    {
        throw new NotImplementedException();
    }

    [HttpPost("managers/{id:guid}/admissions")]
    [SwaggerOperation(Summary = "Assign an admission to manager")]
    public void AssignManager(Guid id, [FromBody] Guid admissionId)
    {
        throw new NotImplementedException();
    }
}