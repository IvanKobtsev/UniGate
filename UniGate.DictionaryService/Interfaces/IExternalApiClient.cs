using UniGate.DictionaryService.DTOs;

namespace UniGate.DictionaryService.Interfaces;

public interface IExternalApiClient
{
    public Task<List<EducationLevelDto>> ImportEducationLevelsAsync();
    public Task<List<FacultyDto>> ImportFacultiesAsync();
    public Task<List<EducationDocumentTypeDto>> ImportEducationDocumentTypesAsync();
    public Task<EducationProgramsDto> ImportEducationProgramsAsync(int page = 1);
}