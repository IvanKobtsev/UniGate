using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Controllers;

[ApiController]
[Route("/api/v1/applicants")]
public class ApplicantController(IUserService userService, IApplicantService applicantService)
    : ControllerBase
{
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Applicant registration")]
    public async Task<IActionResult> Register([FromBody] RegisterApplicantDto registerApplicantDto)
    {
        return (await applicantService.RegisterApplicant(registerApplicantDto)).GetActionResult();
    }

    [Authorize(Roles = "Applicant")]
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Get current applicant's data")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await applicantService.GetApplicantById(Guid.Parse(userId))).GetActionResult();
    }

    [Authorize(Roles = "Applicant")]
    [HttpPut("me")]
    [SwaggerOperation(Summary = "Update current applicant's data")]
    public async Task<IActionResult> UpdateCurrentApplicantProfile(UpdateApplicantDto applicant)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await applicantService.UpdateApplicant(Guid.Parse(userId), applicant)).GetActionResult();
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get applicant's profile by id")]
    public async Task<IActionResult> GetApplicantProfile(Guid id)
    {
        return (await applicantService.GetApplicantById(id)).GetActionResult();
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update applicant's profile by id")]
    public async Task<IActionResult> UpdateApplicantProfile(Guid id, UpdateApplicantDto applicant)
    {
        return (await applicantService.UpdateApplicant(id, applicant)).GetActionResult();
    }
}