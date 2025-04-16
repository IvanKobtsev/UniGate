namespace UniGate.DictionaryService.DTOs.Dictionary;

public class EducationDocumentTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public EducationLevelDto EducationLevel { get; set; }
    public List<EducationLevelDto> NextEducationLevels { get; set; }
}