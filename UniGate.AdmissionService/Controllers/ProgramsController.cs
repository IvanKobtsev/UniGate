using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs;
using UniGateAPI.DTOs.Request;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api")]
public class ProgramsController : ControllerBase
{
    [HttpPost("programs/{id:guid}/apply")]
    [SwaggerOperation(Summary = "Apply for an education program")]
    public void ApplyForProgram()
    {
        throw new NotImplementedException();
    }

    [HttpPut("me/chosen-programs/{id:guid}")]
    [SwaggerOperation(Summary = "Change the priority of an education program")]
    public void ChangeProgramPriority([FromRoute] Guid id, [FromBody] ChangeProgramPriorityDto changeProgramPriorityDto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("me/chosen-programs/{id:guid}")]
    [SwaggerOperation(Summary = "Remove education program from chosen")]
    public void DeleteProgramFromChosen([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("applicants/{applicantId:guid}/chosen-programs/{programId:guid}")]
    [SwaggerOperation(Summary = "Remove education program of applicant from chosen")]
    public void DeleteProgramOfApplicantFromChosen([FromRoute] Guid applicantId, [FromRoute] Guid programId)
    {
        throw new NotImplementedException();
    }
}