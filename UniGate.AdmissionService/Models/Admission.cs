using System.ComponentModel.DataAnnotations.Schema;
using UniGateAPI.Enums;

namespace UniGateAPI.Models;

public class Admission
{
    public Guid? ManagerId;
    public required Guid ApplicantId { get; set; }
    public DateTime CreateTime { get; set; }
    [ForeignKey("ManagerId")] public ManagerReference? ManagerReference { get; set; }
    [ForeignKey("ApplicantId")] public ApplicantReference? ApplicantReference { get; set; }
    public Guid Id { get; set; }
    public AdmissionStatus Status { get; set; }
    public AdmissionType AdmissionType { get; set; }
    public List<ProgramPreference> ProgramPreferences { get; set; } = [];
    public DateTime LastUpdateTime { get; set; }
}