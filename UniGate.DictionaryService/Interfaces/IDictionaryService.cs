using UniGate.DictionaryService.DTOs;

namespace UniGate.DictionaryService.Interfaces;

public interface IDictionaryService
{
    public Task<List<EducationLevelDto>> GetEducationLevels();
    public Task<List<EducationDocumentTypeDto>> GetEducationDocumentTypes();

    public Task<EducationProgramsDto> GetEducationPrograms(int currentPage = 1, int pageSize = 10,
        Guid? facultyId = null, int? educationLevelId = null, string? educationForm = null, string? language = null,
        string? programSearch = null);

    public Task<List<FacultyDto>> GetFaculties();
}