using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class ImportStateMapper
{
    public static ImportStateDto ToDto(this ImportState importState)
    {
        return new ImportStateDto
        {
            ImportStartDateTime = importState.ImportStartDateTime,
            ImportEndDateTime = importState.ImportEndDateTime,
            ImportType = importState.ImportType,
            ImportStatus = importState.ImportStatus
        };
    }

    public static List<ImportStateDto> ToDtos(this List<ImportState> importStates)
    {
        return importStates.Select(ToDto).ToList();
    }
}