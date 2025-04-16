namespace UniGate.DictionaryService.DTOs;

public class EducationDocumentTypeModel
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; }
    public EducationLevelModel EducationLevel { get; set; }
    public List<EducationLevelModel> NextEducationLevels { get; set; }
}