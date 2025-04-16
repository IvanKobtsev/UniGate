using UniGateAPI.DTOs.Common;
using UniGateAPI.Enums;

namespace UniGateAPI.DTOs.Response;

public class MyAdmissionDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public AdmissionType AdmissionType { get; set; }
    public AdmissionStatus Status { get; set; }
    public required List<ProgramPreferenceDto> ProgramPreferences { get; set; }
}