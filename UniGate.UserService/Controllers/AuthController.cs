using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.Common.Exceptions;
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
        return Ok(await usersService.Login(loginDto));
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "User registration")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        return Ok(await usersService.Register(registerDto));
    }

    [Authorize]
    [HttpPost("logout")]
    [SwaggerOperation(Summary = "User logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new NotFoundException("User ID not found in claims.");

        await tokenStore.RevokeRefreshTokenAsync(userId);

        return Ok();
    }

    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refresh access token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
    {
        return Ok(await usersService.RefreshToken(refreshTokenDto.Token));
    }
}