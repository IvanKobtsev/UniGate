using UniGate.DictionaryService.Enums;

namespace UniGate.DictionaryService.DTOs;

public class ImportStateDto
{
    public DateTime ImportStartDateTime { get; set; }
    public DateTime? ImportEndDateTime { get; set; }
    public ImportType ImportType { get; set; }
    public ImportStatus ImportStatus { get; set; }
}