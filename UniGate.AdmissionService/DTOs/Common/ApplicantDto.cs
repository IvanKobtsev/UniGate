namespace UniGateAPI.DTOs.Common;

public class ApplicantDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public List<DocumentDto> Documents { get; set; } = [];
}