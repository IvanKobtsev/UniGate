using UniGate.Common.Enums;

namespace UniGate.UserService.DTOs.Requests;

public class UpdateApplicantDto
{
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Citizenship { get; set; } = string.Empty;
}