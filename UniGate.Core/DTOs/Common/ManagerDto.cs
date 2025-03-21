using UniGateAPI.DTOs;
using UniGateAPI.Models;

public class ManagerDto
{
    public Guid Id { get; set; }
    // public DateTime CreateTime { get; set; }
    public FacultyDto? AssignedFaculty { get; set; }
    public List<AdmissionDto> AssignedAdmissions { get; set; } = [];
}