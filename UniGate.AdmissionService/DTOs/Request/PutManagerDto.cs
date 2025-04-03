namespace UniGateAPI.DTOs;

public class PutManagerDto
{
    public Guid? AssignedFacultyId { get; set; }
    public List<AdmissionDto> AssignedAdmissions { get; set; } = [];
}