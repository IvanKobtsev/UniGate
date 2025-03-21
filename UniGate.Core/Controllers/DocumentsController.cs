using Microsoft.AspNetCore.Mvc;
using UniGateAPI.DTOs.Request;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api")]
public class DocumentsController : ControllerBase
{
    [HttpPost("me/documents")]
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