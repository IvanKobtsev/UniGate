namespace UniGateAPI.Models;

public class Manager
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public Faculty? AssignedFaculty { get; set; }
    public List<Admission> AssignedAdmissions { get; set; } = [];
}