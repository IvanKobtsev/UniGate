using UniGate.Common.DTOs;
using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Interfaces;
using UniGate.DictionaryService.Mappers;

namespace UniGate.DictionaryService.Services;

public class DictionaryService(IDictionaryRepository dictionaryRepository) : IDictionaryService
{
    public async Task<List<EducationLevelDto>> GetEducationLevels()
    {
        return (await dictionaryRepository.GetEducationLevels()).ToDtos();
    }

    public async Task<List<EducationDocumentTypeDto>> GetEducationDocumentTypes()
    {
        var educationLevelsDictionary = await dictionaryRepository.GetEducationLevelsGuidToIntDictionary();

        return (await dictionaryRepository.GetEducationDocumentTypes()).ToDtos(educationLevelsDictionary);
    }

    public async Task<List<FacultyDto>> GetFaculties()
    {
        return (await dictionaryRepository.GetFaculties()).ToDtos();
    }

    public async Task<EducationProgramsPagedListDto> GetEducationPrograms(int currentPage = 1, int pageSize = 10,
        Guid? facultyId = null, int? educationLevelId = null, string? educationForm = null, string? language = null,
        string? programSearch = null)
    {
        var paginatedList = await dictionaryRepository.GetPaginatedEducationPrograms(currentPage, pageSize, facultyId,
            educationLevelId,
            educationForm, language, programSearch);

        return new EducationProgramsPagedListDto
        {
            Pagination = new PaginationDto
            {
                Count = paginatedList.PagesCount,
                Current = paginatedList.PageIndex,
                Size = paginatedList.PageSize
            },
            Programs = paginatedList.Items.ToDtos()
        };
    }
}