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

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Change the priority of an education program")]
    public void ChangeProgramPriority([FromRoute] Guid id, [FromBody] ChangeProgramPriorityDto changeProgramPriorityDto)
    {
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Remove education program from chosen")]
    public void DeleteProgramFromChosen([FromRoute] Guid id)
    {
    }
}