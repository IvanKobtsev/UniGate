using Microsoft.AspNetCore.Mvc;
using UniGate.Common.Enums;

namespace UniGate.Common.Utilities;

public class Result<T> : Result
{
    public T? Data { get; set; }

    public override IActionResult GetActionResult()
    {
        return Code == HttpCode.Ok ? new OkObjectResult(Data) : base.GetActionResult();
    }
}