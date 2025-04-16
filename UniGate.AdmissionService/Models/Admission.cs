using System.ComponentModel.DataAnnotations.Schema;
using UniGateAPI.Enums;

namespace UniGateAPI.Models;

public class Admission
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public AdmissionType AdmissionType { get; set; }
    public AdmissionStatus Status { get; set; }
    public required Guid ApplicantId { get; set; }
    [ForeignKey("ApplicantId")] public ApplicantReference ApplicantReference { get; set; }
    public Guid? ManagerId { get; set; }
    [ForeignKey("ManagerId")] public ManagerReference? ManagerReference { get; set; }
    public List<ProgramPreference> ProgramPreferences { get; set; } = [];
}