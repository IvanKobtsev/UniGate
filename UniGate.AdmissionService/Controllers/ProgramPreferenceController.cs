using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs.Request;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api/v1/program-preferences")]
public class ProgramPreferenceController(IApplicantService applicantService) : ControllerBase
{
    [Authorize(Roles = "Applicant")]
    [HttpPost("")]
    [SwaggerOperation(Summary = "Apply for an education program as applicant")]
    public async Task<IActionResult> ApplyForProgram([FromBody] ApplyForEducationProgramDto educationProgramDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await applicantService.ChooseEducationProgramAsApplicant(Guid.Parse(userId),
                educationProgramDto.AdmissionId,
                educationProgramDto.EducationProgramId))
            .GetActionResult();
    }

    [Authorize]
    [HttpPatch("{programPreferenceId:guid}")]
    [SwaggerOperation(Summary = "Change the priority of chosen education program")]
    public async Task<IActionResult> ChangeProgramPriority([FromRoute] Guid programPreferenceId,
        [FromBody] ChangeProgramPriorityDto changeProgramPriorityDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roles = User.FindFirst(ClaimTypes.Role)?.Value.Split(',').ToList();

        return userId == null || roles == null
            ? Unauthorized()
            : (await applicantService.ChangePriorityOfProgramForApplicant(Guid.Parse(userId), roles,
                programPreferenceId,
                changeProgramPriorityDto.NewPriority)).GetActionResult();
    }

    [Authorize]
    [HttpDelete("{programPreferenceId:guid}")]
    [SwaggerOperation(Summary = "Remove education program from chosen")]
    public async Task<IActionResult> DeleteProgramFromChosen([FromRoute] Guid programPreferenceId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roles = User.FindFirst(ClaimTypes.Role)?.Value.Split(',').ToList();

        return userId == null || roles == null
            ? Unauthorized()
            : (await applicantService.DeleteProgramPreference(Guid.Parse(userId), roles,
                programPreferenceId)).GetActionResult();
    }
}