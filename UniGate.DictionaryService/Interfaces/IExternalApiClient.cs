using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.DTOs.External;

namespace UniGate.DictionaryService.Interfaces;

public interface IExternalApiClient
{
    public Task<List<EducationLevelModel>> ImportEducationLevelsAsync();
    public Task<List<FacultyModel>> ImportFacultiesAsync();
    public Task<List<EducationDocumentTypeModel>> ImportEducationDocumentTypesAsync();
    public Task<ProgramPagedListModel> ImportEducationProgramsAsync(int page = 1);
}