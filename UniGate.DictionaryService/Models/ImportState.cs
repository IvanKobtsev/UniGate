using UniGate.DictionaryService.Enums;

namespace UniGate.DictionaryService.Models;

public class ImportState
{
    public Guid Id { get; set; }
    public ImportStatus ImportStatus { get; set; }
    public ImportType ImportType { get; set; }
    public DateTime ImportStartDateTime { get; set; }
    public DateTime? ImportEndDateTime { get; set; }
}