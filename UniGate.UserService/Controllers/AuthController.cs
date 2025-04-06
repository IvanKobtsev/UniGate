using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Controllers;

[ApiController]
[Route("/api")]
public class AuthController(IUsersService usersService, ITokenStore tokenStore)
    : ControllerBase
{
    [HttpPost("login")]
    [SwaggerOperation(Summary = "User login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        return (await usersService.Login(loginDto)).GetActionResult();
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "User registration")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        return (await usersService.Register(registerDto)).GetActionResult();
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
        return (await usersService.RefreshToken(refreshTokenDto.Token)).GetActionResult();
    }
}