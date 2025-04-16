namespace UniGate.ServiceBus.DTOs;

public class UpdateManagerDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsChief { get; set; }
}