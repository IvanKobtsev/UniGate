namespace UniGateAPI.Models;

public class EducationDocumentType
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public required EducationLevel EducationLevel { get; set; }
    public List<EducationLevel>? NextEducationLevels { get; set; }
}