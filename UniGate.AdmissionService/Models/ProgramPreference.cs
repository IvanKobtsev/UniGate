using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniGateAPI.Models;

public class ProgramPreference
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public required Guid AdmissionId { get; set; }

    [ForeignKey("AdmissionId")] public Admission Admission { get; set; }

    public Guid ChosenProgramId { get; set; }
    public Guid FacultyOfChosenProgramId { get; set; }
    [Range(1, int.MaxValue)] public int Priority { get; set; }
}