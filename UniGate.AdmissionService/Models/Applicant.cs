using System.ComponentModel.DataAnnotations;
using UniGateAPI.Enums;

namespace UniGateAPI.Models;

public class Applicant
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }

    [MinLength(1)] [MaxLength(255)] public required string FirstName { get; set; }

    [MinLength(1)] [MaxLength(255)] public required string LastName { get; set; }

    [MinLength(1)] [MaxLength(255)] public string? Patronymic { get; set; }

    [MinLength(1)] [MaxLength(255)] public required string Email { get; set; }

    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }

    [MinLength(1)] [MaxLength(255)] public required string Citizenship { get; set; }

    [Phone]
    [MinLength(11)]
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }

    public List<Admission> Admissions { get; set; } = [];
    public List<Guid> DocumentsIds { get; set; } = [];
}