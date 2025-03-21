using UniGateAPI.Models;

namespace UniGateAPI.DTOs;

public class EducationProgramDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string EducationForm { get; set; } = string.Empty;
    public required FacultyDto Faculty { get; set; }
    public required EducationLevel EducationLevel { get; set; }
}