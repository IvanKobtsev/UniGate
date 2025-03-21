namespace UniGateAPI.DTOs;

public class ShallowManagerDto
{
    public Guid Id { get; set; }
    // public DateTime CreateTime { get; set; }
    public Guid? AssignedFacultyId { get; set; }
    public List<Guid> AssignedAdmissionsIds { get; set; } = [];
}