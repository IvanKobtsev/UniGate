using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using UniGateAPI.DTOs;

namespace UniGateAPI.Controllers;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api")]
public class ManagersController: ControllerBase
{
    [HttpPost("managers")]
    [SwaggerOperation(Summary = "Create a manager")]
    public Guid CreateManager([FromBody] CreateManagerDto managerDto)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("managers/{id:guid}")]
    [SwaggerOperation(Summary = "Get manager by their id")]
    public ManagerDto GetManager(Guid id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("managers/{id:guid}")]
    [SwaggerOperation(Summary = "Update the profile of manager by their id")]
    public void UpdateManager(Guid id, [FromBody] ManagerDto managerDto)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("managers/{id:guid}")]
    [SwaggerOperation(Summary = "Delete manager by their id")]
    public void DeleteManager(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("managers/me")]
    [SwaggerOperation(Summary = "Get current manager's profile")]
    public ManagerDto GetCurrentManager()
    {
        throw new NotImplementedException();
    }
}