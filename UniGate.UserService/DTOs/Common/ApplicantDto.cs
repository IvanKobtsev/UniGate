using UniGate.Common.Enums;

namespace UniGate.UserService.DTOs.Common;

public class ApplicantDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Citizenship { get; set; } = string.Empty;
}