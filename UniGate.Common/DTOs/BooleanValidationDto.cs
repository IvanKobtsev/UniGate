using UniGate.Common.Enums;

namespace UniGate.Common.DTOs;

public class BooleanValidationDto
{
    public bool? IsValid { get; set; }
    public HttpCode Code { get; set; } = HttpCode.Ok;
    public string? ErrorMessage { get; set; }
}