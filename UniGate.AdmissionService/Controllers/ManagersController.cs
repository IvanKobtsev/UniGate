using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs.Common;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api/v1/managers")]
public class ManagersController(IManagerService managerService) : ControllerBase
{
    [Authorize(Roles = "Manager,ChiefManager")]
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Get current manager's profile with assigned faculty and admissions")]
    public async Task<IActionResult> GetCurrentManager()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await managerService.GetManagerProfile(Guid.Parse(userId))).GetActionResult();
    }

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