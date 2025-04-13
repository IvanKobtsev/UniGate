using System.ComponentModel;

namespace UniGate.UserService.DTOs.Requests;

public class UpdatePasswordDto
{
    [PasswordPropertyText] public required string CurrentPassword { get; set; }
    [PasswordPropertyText] public required string NewPassword { get; set; }
}