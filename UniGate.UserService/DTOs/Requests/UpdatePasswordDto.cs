namespace UniGate.UserService.DTOs.Requests;

public class UpdatePasswordDto
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}