using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Controllers;

[ApiController]
[Route("/api/v1/managers")]
public class ManagerController(IManagerService managerService, IUserService userService, ITokenStore tokenStore)
    : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("")]
    [SwaggerOperation(Summary = "Create new manager")]
    public async Task<IActionResult> CreateManager([FromBody] RegisterUserDto registerUserDto)
    {
        return (await managerService.CreateManager(registerUserDto)).GetActionResult();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("chiefs")]
    [SwaggerOperation(Summary = "Create new chief manager")]
    public async Task<IActionResult> CreateChiefManager([FromBody] RegisterUserDto registerUserDto)
    {
        return (await managerService.CreateManager(registerUserDto, true)).GetActionResult();
    }

    // !———— Also "copy" to AdmissionService ————!
    //
    [Authorize(Roles = "Admin,ChiefManager")]
    [HttpGet("")]
    [SwaggerOperation(Summary = "Get all managers paginated")]
    public async Task<IActionResult> GetPaginatedManagers([FromQuery] int currentPage = 1,
        [FromQuery] int pageSize = 10, [FromQuery] bool chiefsOnly = false)
    {
        return (await managerService.GetPaginatedManagers(currentPage, pageSize, chiefsOnly)).GetActionResult();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete manager's profile by id")]
    public async Task<IActionResult> DeleteManager(Guid id)
    {
        return (await userService.DeleteProfile(id)).GetActionResult();
    }
}