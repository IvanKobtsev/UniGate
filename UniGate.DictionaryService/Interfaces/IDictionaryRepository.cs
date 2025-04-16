using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Interfaces;

public interface IDictionaryRepository
{
    public Task StoreEducationLevels(List<EducationLevel> educationLevels);
    public Task StoreEducationDocumentTypes(List<EducationDocumentType> educationDocumentTypes);
    public Task StoreEducationPrograms(List<EducationProgram> educationPrograms);
    public Task StoreFaculties(List<Faculty> facultyDtos);

    public Task<PaginatedList<EducationLevel>> GetPaginatedEducationLevels(int currentPage, int pageSize,
        string? levelName);

    public Task<PaginatedList<EducationDocumentType>> GetPaginatedEducationDocumentTypes(int currentPage,
        int pageSize,
        string? documentName);

    public Task<PaginatedList<EducationProgram>> GetPaginatedEducationPrograms(int currentPage, int pageSize,
        Guid? facultyId, int? educationLevelId, string? educationForm, string? language,
        string? programNameOrCode);

    public Task<PaginatedList<Faculty>> GetPaginatedFaculties(int currentPage, int pageSize,
        string? facultyName);

    public Task<Guid> StartImport(ImportType importType);
    public Task FinishImport(Guid importId, ImportStatus importStatus);
    public Task ClearDatabase();

    public Task<PaginatedList<ImportState>> GetImportStates(int currentPage, int pageSize,
        Sorting sorting);

    public Task<ImportState?> GetLastImportState();
    public Task<Dictionary<int, Guid>> GetEducationLevelsIntToGuidDictionary();
    public Task<Dictionary<Guid, int>> GetEducationLevelsGuidToIntDictionary();
    public Task<EducationProgram?> GetEducationProgram(Guid programId);
    public Task<EducationDocumentType?> GetEducationDocumentType(Guid documentTypeId);
    public Task<List<EducationDocumentType>> GetAllEducationDocumentTypes();
}