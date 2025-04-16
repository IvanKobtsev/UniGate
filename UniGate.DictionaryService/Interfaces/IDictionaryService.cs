using UniGate.Common.DTOs;
using UniGate.DictionaryService.DTOs.Dictionary;

namespace UniGate.DictionaryService.Interfaces;

public interface IDictionaryService
{
    public Task<PaginatedListDto<EducationLevelDto>> GetEducationLevels(int currentPage, int pageSize,
        string? levelName);

    public Task<PaginatedListDto<EducationDocumentTypeDto>> GetEducationDocumentTypes(int currentPage, int pageSize,
        string? documentTypeName);

    public Task<PaginatedListDto<EducationProgramDto>> GetEducationPrograms(int currentPage, int pageSize,
        Guid? facultyId, int? educationLevelId, string? educationForm, string? language,
        string? programNameOrCode);

    public Task<PaginatedListDto<FacultyDto>> GetFaculties(int currentPage, int pageSize, string? facultyName);
}