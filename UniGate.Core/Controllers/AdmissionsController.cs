using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGateAPI.DTOs.Response;
using UniGateAPI.Enums;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api")]
public class AdmissionsController : ControllerBase
{
    [HttpGet("admissions")]
    [SwaggerOperation(Summary = "Get all admissions")]
    public PaginatedAdmissionsList GetAdmissions([FromQuery] string name, [FromQuery] Guid programId,
        [FromQuery] List<Guid> faculties, [FromQuery] AdmissionStatus admissionStatus, [FromQuery] bool onlyNotTaken,
        [FromQuery] bool onlyMine, [FromQuery] Sorting sorting, [FromQuery] int page, [FromQuery] int pageSize)
    {
        throw new NotImplementedException();
    }

    [HttpPost("me/admissions")]
    [SwaggerOperation(Summary = "Take admission for managing by current user")]
    public void TakeAdmission([FromBody] Guid admissionId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("me/admissions/{admissionId:guid}")]
    [SwaggerOperation(Summary = "Remove an admission from current manager")]
    public void RemoveAdmissionFromTaken(Guid admissionId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("admissions/{admissionId:guid}")]
    [SwaggerOperation(Summary = "Update status of admission")]
    public void UpdateAdmissionStatus(Guid admissionId, [FromBody] AdmissionStatus status)
    {
        throw new NotImplementedException();
    }
}