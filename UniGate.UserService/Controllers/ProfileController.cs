using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.Common.Exceptions;
using UniGate.UserService.DTOs.Common;
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
    public async Task<ProfileDto> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new UnauthorizedException("Unauthorized");

        return await usersService.GetProfileDto(userId);
    }

    [Authorize]
    [HttpPut("")]
    [SwaggerOperation(Summary = "Update current user's profile data")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profileDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new UnauthorizedException("Unauthorized");

        await usersService.UpdateProfile(userId, profileDto);

        return Ok();
    }

    [Authorize]
    [HttpPut("password")]
    [SwaggerOperation(Summary = "Send a request to update a password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        if (updatePasswordDto.NewPassword == updatePasswordDto.CurrentPassword)
            return BadRequest("New password must be different from current");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new UnauthorizedException("Unauthorized");

        await usersService.UpdatePassword(userId, updatePasswordDto);

        return Ok();
    }
}