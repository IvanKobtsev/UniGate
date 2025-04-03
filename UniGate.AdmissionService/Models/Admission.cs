using UniGateAPI.Enums;

namespace UniGateAPI.Models;

public class Admission
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public AdmissionStatus Status { get; set; }
    public required Applicant Applicant { get; set; }
    public List<ProgramPreference> ProgramPreferences { get; set; } = [];
}