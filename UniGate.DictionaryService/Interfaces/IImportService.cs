using UniGate.Common.DTOs;
using UniGate.Common.Enums;
using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Enums;

namespace UniGate.DictionaryService.Interfaces;

public interface IImportService
{
    public Task Import(ImportType importType);
    public Task<bool> IsImporting();

    Task<PaginatedListDto<ImportStateDto>> GetImportHistory(int currentPage, int pageSize,
        Sorting sorting);

    public Task<ImportStateDto?> GetImportStatus();
}