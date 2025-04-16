using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api/v1/applicants")]
public class ApplicantController(IApplicantService applicantService) : ControllerBase
{
    [Authorize(Roles = "Applicant")]
    [HttpGet("me/admissions")]
    [SwaggerOperation(Summary = "Get admissions of current user")]
    public async Task<IActionResult> GetMyAdmissions()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await applicantService.GetAdmissionsOfUser(Guid.Parse(userId))).GetActionResult();
    }

    [Authorize(Roles = "Manager,ChiefManager,Admin")]
    [HttpGet("{applicantId:guid}/admissions")]
    [SwaggerOperation(Summary = "Get admissions of specified user")]
    public async Task<IActionResult> GetMyAdmissions([FromRoute] Guid applicantId)
    {
        return (await applicantService.GetAdmissionsOfUser(applicantId)).GetActionResult();
    }
}