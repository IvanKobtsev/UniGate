namespace UniGate.FileService.Models;

public class EducationDocument : Document
{
    public string Name { get; set; } = string.Empty;
    public Guid EducationDocumentTypeId { get; set; }
}