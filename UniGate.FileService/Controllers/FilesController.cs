using Microsoft.AspNetCore.Mvc;
using UniGate.FileService.DTOs.Common;

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

    // [HttpPost("files")]
    // public Guid CreateFile([FromBody] CreateFileDto file)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // [HttpPut("files/{id:guid}")]
    // public void UpdateFile(Guid id, [FromBody] UpdateFileDto file)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // [HttpDelete("files/{id:guid}")]
    // public void DeleteFile(Guid id)
    // {
    //     throw new NotImplementedException();
    // }
}