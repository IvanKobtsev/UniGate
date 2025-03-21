using Swashbuckle.AspNetCore.Annotations;
using UniGate.AuthenticationService.DTOs.Common;
using UniGate.AuthenticationService.DTOs.Requests;

namespace UniGate.AuthenticationService.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController(Services.AuthenticationService authenticationService) : ControllerBase
{
    private readonly Services.AuthenticationService _authenticationService = authenticationService;
    
    [HttpGet("/validate")]
    [SwaggerOperation(Summary = "Validate token and get user id")]
    public ValidateTokenDto ValidateToken([FromBody] string token)
    {
        throw new NotImplementedException();
    }
}