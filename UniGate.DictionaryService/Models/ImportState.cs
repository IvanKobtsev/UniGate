using UniGate.DictionaryService.Enums;

namespace UniGate.DictionaryService.Models;

public class ImportState
{
    public Guid Id { get; set; }
    public ImportStatus ImportStatus { get; set; }
    public DateTime ImportDateTime { get; set; }
}