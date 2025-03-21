namespace UniGateAPI.DTOs;

public class PutManagerDto
{
    public FacultyDto? AssignedFaculty { get; set; }
    public List<AdmissionDto> AssignedAdmissions { get; set; } = [];
}