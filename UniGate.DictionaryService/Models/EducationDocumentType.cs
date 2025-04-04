using System.ComponentModel.DataAnnotations.Schema;

namespace UniGate.DictionaryService.Models;

public class EducationDocumentType
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid EducationLevelId { get; set; }
    [ForeignKey("EducationLevelId")] public EducationLevel EducationLevel { get; set; }
    public List<EducationLevelAccess> NextEducationLevels { get; set; }
}