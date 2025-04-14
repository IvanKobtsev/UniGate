using System.ComponentModel.DataAnnotations;

namespace UniGateAPI.Models;

public class ManagerReference
{
    [Key] public Guid UserId { get; set; }

    public bool IsChief { get; set; }
    [MaxLength(152)] public required string FullName { get; set; }
    public Guid? AssignedFacultyId { get; set; }
    public List<Admission> AssignedAdmissions { get; set; } = [];
}