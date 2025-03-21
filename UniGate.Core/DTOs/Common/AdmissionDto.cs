using UniGateAPI.Enums;

namespace UniGateAPI.DTOs;

public class AdmissionDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public AdmissionStatus Status { get; set; }
    public Guid ApplicantId { get; set; }
    public List<ProgramPreferenceDto> ProgramPreferences { get; set; } = [];
}