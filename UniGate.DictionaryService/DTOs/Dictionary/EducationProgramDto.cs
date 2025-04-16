namespace UniGate.DictionaryService.DTOs.Dictionary;

public class EducationProgramDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Language { get; set; }
    public string EducationForm { get; set; }
    public FacultyDto Faculty { get; set; }
    public EducationLevelDto EducationLevel { get; set; }
}