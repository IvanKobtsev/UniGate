using UniGateAPI.Enums;

namespace UniGateAPI.DTOs.Common;

public class AdmissionFullDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public AdmissionStatus Status { get; set; }
    public AdmissionType AdmissionType { get; set; }
    public required ApplicantLightDto Applicant { get; set; }
    public List<ProgramPreferenceDto> ProgramPreferences { get; set; } = [];
}