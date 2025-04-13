using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController(IUserService userService, ITokenStore tokenStore)
    : ControllerBase
{
    [HttpPost("login")]
    [SwaggerOperation(Summary = "User login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        return (await userService.Login(loginDto)).GetActionResult();
    }

    [Authorize]
    [HttpPost("logout")]
    [SwaggerOperation(Summary = "User logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null ? Unauthorized() : (await tokenStore.RevokeRefreshTokenAsync(userId)).GetActionResult();
    }

    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refresh access token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
    {
        return (await userService.RefreshToken(refreshTokenDto.Token)).GetActionResult();
    }
}