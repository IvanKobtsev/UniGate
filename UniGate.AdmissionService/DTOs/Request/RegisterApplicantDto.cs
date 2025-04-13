using UniGate.Common.Enums;

namespace UniGateAPI.DTOs.Request;

public class RegisterApplicantDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Citizenship { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<NewProgramPreferenceDto> ProgramPreferences { get; set; } = [];
}