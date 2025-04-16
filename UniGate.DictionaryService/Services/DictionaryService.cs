using UniGate.Common.DTOs;
using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Interfaces;
using UniGate.DictionaryService.Mappers;

namespace UniGate.DictionaryService.Services;

public class DictionaryService(IDictionaryRepository dictionaryRepository) : IDictionaryService
{
    public async Task<PaginatedListDto<EducationProgramDto>> GetEducationPrograms(int currentPage, int pageSize,
        Guid? facultyId, int? educationLevelId, string? educationForm, string? language,
        string? programNameOrCode)
    {
        return (await dictionaryRepository.GetPaginatedEducationPrograms(currentPage, pageSize, facultyId,
            educationLevelId,
            educationForm, language, programNameOrCode)).ToPaginatedListDto(items => items.ToDtos());
    }

    public async Task<PaginatedListDto<EducationDocumentTypeDto>> GetEducationDocumentTypes(int currentPage,
        int pageSize,
        string? documentTypeName)
    {
        return (await dictionaryRepository.GetPaginatedEducationDocumentTypes(currentPage, pageSize, documentTypeName))
            .ToPaginatedListDto(items => items.ToDtos());
    }

    public async Task<PaginatedListDto<EducationLevelDto>> GetEducationLevels(int currentPage, int pageSize,
        string? levelName)
    {
        return (await dictionaryRepository.GetPaginatedEducationLevels(currentPage, pageSize, levelName))
            .ToPaginatedListDto(items => items.ToDtos());
    }

    public async Task<PaginatedListDto<FacultyDto>> GetFaculties(int currentPage, int pageSize, string? facultyName)
    {
        return (await dictionaryRepository.GetPaginatedFaculties(currentPage, pageSize, facultyName))
            .ToPaginatedListDto(items => items.ToDtos());
    }
}