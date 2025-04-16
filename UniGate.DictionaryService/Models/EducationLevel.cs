namespace UniGate.DictionaryService.Models;

public class EducationLevel
{
    public Guid Id { get; set; }
    public int IntegerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EducationDocumentType> EducationDocumentTypes { get; set; }
    public List<EducationLevelAccess> EducationLevelAccesses { get; set; }
    public List<EducationProgram> EducationPrograms { get; set; }
}