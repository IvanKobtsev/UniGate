using Microsoft.AspNetCore.Mvc;
using UniGate.Common.Enums;

namespace UniGate.Common.Utilities;

public class Result
{
    public HttpCode Code { get; set; } = HttpCode.Ok;
    public string? Message { get; set; }

    public IActionResult GetActionResult()
    {
        return Code switch
        {
            HttpCode.Ok => new OkResult(),
            HttpCode.Created => new CreatedResult(),
            HttpCode.Accepted => new AcceptedResult(),
            HttpCode.NoContent => new NoContentResult(),
            HttpCode.BadRequest => new BadRequestResult(),
            HttpCode.Unauthorized => new UnauthorizedResult(),
            // case HttpCode.Forbidden:
            //     return
            HttpCode.NotFound => new NotFoundResult(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}