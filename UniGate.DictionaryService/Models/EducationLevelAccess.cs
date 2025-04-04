namespace UniGate.DictionaryService.Models;

public class EducationLevelAccess
{
    public Guid Id { get; set; }
    public Guid EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; }
    public Guid EducationDocumentTypeId { get; set; }
    public EducationDocumentType EducationDocumentType { get; set; }
}