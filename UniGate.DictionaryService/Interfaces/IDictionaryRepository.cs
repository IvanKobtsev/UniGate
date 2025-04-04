using UniGate.DictionaryService.DTOs.Response;

namespace UniGate.DictionaryService.Interfaces;

public interface IDictionaryRepository
{
    public Task StoreEducationLevels(List<EducationLevelDto> educationLevelDtos);
    public Task<List<EducationLevelDto>> RetrieveEducationLevels();
    public Task CommitChanges();
}