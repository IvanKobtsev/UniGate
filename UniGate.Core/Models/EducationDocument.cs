namespace UniGateAPI.Models;

public class EducationDocument : Document
{
    public string Name { get; set; } = string.Empty;
    public required EducationDocumentType Type { get; set; }
}