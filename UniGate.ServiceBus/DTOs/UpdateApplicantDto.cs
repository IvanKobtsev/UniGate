namespace UniGate.ServiceBus.DTOs;

public class UpdateApplicantDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
}