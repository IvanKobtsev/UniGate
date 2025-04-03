using System.ComponentModel.DataAnnotations;

namespace UniGateAPI.Models;

public class ProgramPreference
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    [Required] public required Admission Admission { get; set; }
    public Guid ChosenProgramId { get; set; }
    public int Priority { get; set; }
}