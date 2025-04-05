using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Enums;

namespace UniGate.DictionaryService.Interfaces;

public interface IDictionaryRepository
{
    public Task StoreEducationLevels(List<EducationLevelDto> educationLevelDtos);
    public Task StoreEducationDocumentTypes(List<EducationDocumentTypeDto> educationDocumentTypeDtos);
    public Task StoreEducationPrograms(List<EducationProgramDto> educationProgramDtos);
    public Task StoreFaculties(List<FacultyDto> facultyDtos);
    public Task<List<EducationLevelDto>> RetrieveEducationLevels();
    public Task<List<EducationDocumentTypeDto>> RetrieveEducationDocumentTypes();

    public Task<EducationProgramsDto> RetrieveEducationPrograms(int currentPage = 1, int pageSize = 10,
        Guid? facultyId = null, Guid? educationLevelId = null, string? educationForm = null, string? language = null,
        string? programSearch = null);

    public Task<List<FacultyDto>> RetrieveFaculties();
    public Task<Guid> StartImport(ImportType importType);
    public Task FinishImport(Guid importId, ImportStatus importStatus);
    public Task ClearDatabase();
    public Task<List<ImportStateDto>> RetrieveImportStates();
    public Task<ImportStateDto?> RetrieveLastImportState();
}