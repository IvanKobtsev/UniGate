namespace UniGate.DictionaryService.Models;

public class EducationProgram
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string EducationForm { get; set; } = string.Empty;
    public Guid FacultyId { get; set; }
    public Faculty Faculty { get; set; }
    public Guid EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; }
}