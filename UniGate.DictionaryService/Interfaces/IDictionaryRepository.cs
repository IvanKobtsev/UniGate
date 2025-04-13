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
    public Task<List<EducationLevel>> GetEducationLevels();
    public Task<List<EducationDocumentType>> GetEducationDocumentTypes();

    public Task<PaginatedList<EducationProgram>> GetPaginatedEducationPrograms(int currentPage = 1, int pageSize = 10,
        Guid? facultyId = null, int? educationLevelId = null, string? educationForm = null, string? language = null,
        string? programSearch = null);

    public Task<List<Faculty>> GetFaculties();
    public Task<Guid> StartImport(ImportType importType);
    public Task FinishImport(Guid importId, ImportStatus importStatus);
    public Task ClearDatabase();
    public Task<List<ImportState>> GetImportStates();
    public Task<ImportState?> GetLastImportState();
    public Task<Dictionary<int, Guid>> GetEducationLevelsIntToGuidDictionary();
    public Task<Dictionary<Guid, int>> GetEducationLevelsGuidToIntDictionary();
}