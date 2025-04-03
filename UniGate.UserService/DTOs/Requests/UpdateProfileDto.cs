using System.ComponentModel.DataAnnotations;

namespace UniGate.UserService.DTOs.Requests;

public class UpdateProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;

    [EmailAddress] public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}