using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UniGate.Common.Enums;

namespace UniGate.UserService.DTOs.Requests;

public class RegisterApplicantDto
{
    [EmailAddress] [Required] public string Email { get; set; } = string.Empty;

    [PasswordPropertyText] [Required] public string Password { get; set; } = string.Empty;

    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;

    public string? Patronymic { get; set; }

    [Phone]
    [MinLength(11)]
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }

    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Citizenship { get; set; } = string.Empty;
}