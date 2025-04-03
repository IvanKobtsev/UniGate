using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs;
using UniGateAPI.DTOs.Request;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api")]
public class ApplicantsController(IApplicantService applicantService) : ControllerBase
{
    [HttpGet("applicants/{id:guid}")]
    [SwaggerOperation(Summary = "Get applicant's profile by id")]
    public ApplicantDto GetApplicantProfile(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPut("applicants/{id:guid}")]
    [SwaggerOperation(Summary = "Update applicant's profile by id")]
    public void UpdateApplicantProfile(Guid id, ApplicantDto applicant)
    {
        throw new NotImplementedException();
    }

    [HttpGet("applicants/me")]
    [SwaggerOperation(Summary = "Get current applicant's profile")]
    public ApplicantDto GetCurrentApplicantProfile()
    {
        throw new NotImplementedException();
    }

    [HttpPut("applicants/me")]
    [SwaggerOperation(Summary = "Update current applicant's profile")]
    public void UpdateCurrentApplicantProfile(ApplicantDto applicant)
    {
        throw new NotImplementedException();
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register as an applicant")]
    public void Register([FromBody] RegisterApplicantDto registerApplicantDto)
    {
        var applicantDto = new ApplicantDto
        {
            Id = Guid.NewGuid(),
            FirstName = registerApplicantDto.FirstName,
            LastName = registerApplicantDto.LastName,
            Email = registerApplicantDto.Email
        };

        applicantService.CreateApplicant(applicantDto);

        throw new NotImplementedException();
    }
}