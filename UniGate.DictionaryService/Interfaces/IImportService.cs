using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Enums;

namespace UniGate.DictionaryService.Interfaces;

public interface IImportService
{
    public Task Import(ImportType importType);
    public Task<bool> IsImporting();
    public Task<List<ImportStateDto>> GetImportHistory();
    public Task<ImportStateDto?> GetImportStatus();
}