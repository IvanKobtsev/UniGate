using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Services;

public class DictionaryService(IDictionaryRepository dictionaryRepository) : IDictionaryService
{
    public Task<List<EducationLevelDto>> GetEducationLevels()
    {
        return dictionaryRepository.RetrieveEducationLevels();
    }

    public Task<List<EducationDocumentTypeDto>> GetEducationDocumentTypes()
    {
        return dictionaryRepository.RetrieveEducationDocumentTypes();
    }

    public Task<List<FacultyDto>> GetFaculties()
    {
        return dictionaryRepository.RetrieveFaculties();
    }

    public Task<EducationProgramsDto> GetEducationPrograms(int currentPage = 1, int pageSize = 10,
        Guid? facultyId = null, Guid? educationLevelId = null, string? educationForm = null, string? language = null,
        string? programSearch = null)
    {
        return dictionaryRepository.RetrieveEducationPrograms(currentPage, pageSize, facultyId, educationLevelId,
            educationForm, language, programSearch);
    }
}