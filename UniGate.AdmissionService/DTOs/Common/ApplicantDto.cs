using UniGateAPI.Enums;
using UniGateAPI.Models;

namespace UniGateAPI.DTOs;

public class ApplicantDto
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
    public List<DocumentDto> Documents { get; set; } = [];
}