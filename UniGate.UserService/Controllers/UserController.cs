using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Controllers;

[ApiController]
[Route("/api/v1/users")]
public class UserController(IUserService userService)
    : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Get current user's profile data")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await userService.GetProfileDto(Guid.Parse(userId))).GetActionResult();
    }

    [Authorize]
    [HttpPut("me")]
    [SwaggerOperation(Summary = "Update current user's profile data")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profileDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await userService.UpdateProfile(Guid.Parse(userId), profileDto)).GetActionResult();
    }

    [Authorize]
    [HttpPut("me/password")]
    [SwaggerOperation(Summary = "Update current user's password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        if (updatePasswordDto.NewPassword == updatePasswordDto.CurrentPassword)
            return BadRequest("New password must be different from current");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await userService.UpdatePassword(Guid.Parse(userId), updatePasswordDto)).GetActionResult();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update user's profile by id")]
    public async Task<IActionResult> UpdateManager(Guid id, [FromBody] UpdateProfileDto profileDto)
    {
        return (await userService.UpdateProfile(id, profileDto)).GetActionResult();
    }
}