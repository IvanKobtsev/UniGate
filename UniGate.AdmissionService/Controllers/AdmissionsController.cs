using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs.Request;
using UniGateAPI.Enums;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api/v1/admissions")]
public class AdmissionsController(IApplicantService applicantService, IManagerService managerService) : ControllerBase
{
    [Authorize(Roles = "Applicant")]
    [HttpPost("")]
    [SwaggerOperation(Summary = "Apply as applicant")]
    public async Task<IActionResult> ApplyApplicant(
        [FromBody] CreateAdmissionAsApplicantDto createAdmissionAsApplicantDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await applicantService.CreateAdmissionForApplicant(Guid.Parse(userId),
                createAdmissionAsApplicantDto.AdmissionType))
            .GetActionResult();
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Get admissions with filters (paginated)")]
    public async Task<IActionResult> GetPaginatedAdmissions([FromQuery] string? name, [FromQuery] Guid? programId,
        [FromQuery] List<Guid> faculties, [FromQuery] AdmissionStatus? admissionStatus,
        [FromQuery] bool onlyNotTaken = false,
        [FromQuery] bool onlyMine = false, [FromQuery] Sorting sorting = Sorting.DateDesc,
        [Range(1, int.MaxValue)] [FromQuery] int page = 1,
        [Range(1, int.MaxValue)] [FromQuery] int pageSize = 10)
    {
        return (await applicantService.GetPaginatedAdmissions(name, programId, faculties, admissionStatus, onlyNotTaken,
            onlyMine,
            sorting, page, pageSize)).GetActionResult();
    }

    [Authorize(Roles = "Manager,ChiefManager")]
    [HttpPost("{admissionId:guid}/me")]
    [SwaggerOperation(Summary = "Take admission for managing by current user")]
    public async Task<IActionResult> TakeAdmission([FromRoute] Guid admissionId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await managerService.AssignManagerToAdmission(Guid.Parse(userId), admissionId)).GetActionResult();
    }

    [HttpDelete("{admissionId:guid}/me")]
    [SwaggerOperation(Summary = "Remove an admission from current manager")]
    public async Task<IActionResult> RemoveAdmissionFromTaken(Guid admissionId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId == null
            ? Unauthorized()
            : (await managerService.AssignManagerToAdmission(Guid.Parse(userId), admissionId)).GetActionResult();
    }

    [HttpPatch("{admissionId:guid}")]
    [SwaggerOperation(Summary = "Change admission's status")]
    public void UpdateAdmissionStatus(Guid admissionId, [FromBody] AdmissionStatus status)
    {
        throw new NotImplementedException();
    }
}