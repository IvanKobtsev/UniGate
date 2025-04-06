using Microsoft.AspNetCore.Mvc;
using UniGate.Common.Enums;

namespace UniGate.Common.Utilities;

public class Result<T>
{
    public HttpCode Code { get; set; } = HttpCode.Ok;
    public string? Message { get; set; }
    public T? Data { get; set; }

    public IActionResult GetActionResult()
    {
        return Code switch
        {
            HttpCode.Ok => new OkObjectResult(Data),
            HttpCode.Created => new CreatedResult(),
            HttpCode.Accepted => new AcceptedResult(),
            HttpCode.NoContent => new NoContentResult(),
            HttpCode.BadRequest => new BadRequestObjectResult(new { Message }),
            HttpCode.Unauthorized => new UnauthorizedResult(),
            // case HttpCode.Forbidden:
            //     return
            HttpCode.NotFound => new NotFoundObjectResult(new { Message }),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}