using UniGateAPI.Enums;

namespace UniGateAPI.Models;

public class Applicant
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Citizenship { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public required Admission Admission { get; set; }
    public List<Guid> DocumentsIds { get; set; } = [];
}