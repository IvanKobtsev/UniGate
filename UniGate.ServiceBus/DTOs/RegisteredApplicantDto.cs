namespace UniGate.ServiceBus.DTOs;

public class RegisteredApplicantDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
}