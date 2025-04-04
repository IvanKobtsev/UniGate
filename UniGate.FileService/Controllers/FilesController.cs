using Microsoft.AspNetCore.Mvc;
using UniGate.FileService.DTOs.Common;
using UniGateAPI.DTOs.Request;

namespace UniGate.FileService.Controllers;

[ApiController]
[Route("api")]
public class FilesController : ControllerBase
{
    [HttpGet("documents/{id:guid}")]
    public FileDto GetFile(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("documents/my")]
    public void UploadFileAsApplicant([FromBody] UploadFileDto file)
    {
        throw new NotImplementedException();
    }

    [HttpPost("applicants/{applicantId:guid}/documents")]
    public void UploadFileForApplicant(Guid applicantId, [FromBody] UploadFileDto file)
    {
        throw new NotImplementedException();
    }

    [HttpPut("documents/{id:guid}")]
    public void UpdateDocument(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("documents/{id:guid}")]
    public void DeleteDocument(Guid id)
    {
        throw new NotImplementedException();
    }
}