namespace UniGateAPI.DTOs.Common;

public class ManagerDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Guid? AssignedFacultyId { get; set; }
    public List<AdmissionLightDto> AssignedAdmissions { get; set; } = [];
}