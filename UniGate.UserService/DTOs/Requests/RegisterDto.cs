using System.ComponentModel.DataAnnotations;

namespace UniGate.UserService.DTOs.Requests;

public class RegisterDto
{
    [EmailAddress] [Required] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;

    public string? Patronymic { get; set; }
    public string? PhoneNumber { get; set; }
}