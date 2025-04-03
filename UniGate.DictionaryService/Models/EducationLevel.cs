namespace UniGate.DictionaryService.Models;

public class EducationLevel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EducationDocumentType> ApplicableDocumentTypes { get; set; } = [];
}