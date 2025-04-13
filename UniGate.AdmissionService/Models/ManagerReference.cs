using System.ComponentModel.DataAnnotations;

namespace UniGateAPI.Models;

public class ManagerReference
{
    [Key] public Guid Id { get; set; }
    public bool IsChief { get; set; }
    public Guid? AssignedFacultyId { get; set; }
    public List<Admission> AssignedAdmissions { get; set; } = [];
}