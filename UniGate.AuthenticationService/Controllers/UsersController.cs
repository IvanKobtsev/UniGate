using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.AuthenticationService.DTOs.Common;
using UniGate.AuthenticationService.DTOs.Requests;

namespace UniGate.AuthenticationService.Controllers;

[ApiController]
[Route("/api")]
public class UsersController : ControllerBase
{
    [HttpGet("profile")]
    [SwaggerOperation(Summary = "Get current user's profile data")]
    public ProfileDto CreateManager()
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("profile")]
    [SwaggerOperation(Summary = "Update current user's profile data")]
    public void UpdateManager([FromBody] ProfileDto profileDto)
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("profile/password")]
    [SwaggerOperation(Summary = "Send a request to update a password")]
    public void UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("login")]
    [SwaggerOperation(Summary = "User login")]
    public TokenDto Login([FromBody] LoginDto loginDto)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("register")]
    [SwaggerOperation(Summary = "User registration")]
    public TokenDto Register([FromBody] RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("logout")]
    [SwaggerOperation(Summary = "User logout")]
    public void Logout()
    {
        throw new NotImplementedException();
    }
}