using UniGateAPI.DTOs;

public class ManagerDto
{
    public Guid Id { get; set; }

    // public DateTime CreateTime { get; set; }
    public Guid AssignedFacultyId { get; set; }
    public List<AdmissionDto> AssignedAdmissions { get; set; } = [];
}