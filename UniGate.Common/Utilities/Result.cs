using Microsoft.AspNetCore.Mvc;
using UniGate.Common.Enums;

namespace UniGate.Common.Utilities;

public class Result
{
    public HttpCode Code { get; set; } = HttpCode.Ok;
    public string? Message { get; set; }

    public bool IsFailed => (int)Code / 100 > 2;

    public IActionResult GetActionResult()
    {
        return Code switch
        {
            HttpCode.Ok => new OkResult(),
            HttpCode.Created => new CreatedResult(),
            HttpCode.Accepted => new AcceptedResult(),
            HttpCode.NoContent => new NoContentResult(),
            HttpCode.BadRequest => new BadRequestObjectResult(new { Message }),
            HttpCode.Unauthorized => new UnauthorizedResult(),
            HttpCode.NotFound => new NotFoundObjectResult(new { Message }),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}