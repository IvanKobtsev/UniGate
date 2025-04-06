using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Controllers;

[ApiController]
[Route("/api/profile")]
public class ProfileController(IUsersService usersService)
    : ControllerBase
{
    [Authorize]
    [HttpGet("")]
    [SwaggerOperation(Summary = "Get current user's profile data")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null ? Unauthorized() : (await usersService.GetProfileDto(userId)).GetActionResult();
    }

    [Authorize]
    [HttpPut("")]
    [SwaggerOperation(Summary = "Update current user's profile data")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profileDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await usersService.UpdateProfile(userId, profileDto)).GetActionResult();
    }

    [Authorize]
    [HttpPut("password")]
    [SwaggerOperation(Summary = "Send a request to update a password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        if (updatePasswordDto.NewPassword == updatePasswordDto.CurrentPassword)
            return BadRequest("New password must be different from current");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await usersService.UpdatePassword(userId, updatePasswordDto)).GetActionResult();
    }
}