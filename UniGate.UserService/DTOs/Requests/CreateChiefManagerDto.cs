using System.ComponentModel.DataAnnotations;

namespace UniGate.UserService.DTOs.Requests;

public class CreateChiefManagerDto
{
    [EmailAddress] [Required] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;

    public string? Patronymic { get; set; }

    [Phone]
    [MinLength(11)]
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }
}