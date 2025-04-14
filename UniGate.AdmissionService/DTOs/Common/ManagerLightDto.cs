namespace UniGateAPI.DTOs.Common;

public class ManagerLightDto
{
    public Guid Id { get; set; }

    // public DateTime CreateTime { get; set; }
    public Guid? AssignedFacultyId { get; set; }
    public List<Guid> AssignedAdmissionsIds { get; set; } = [];
}