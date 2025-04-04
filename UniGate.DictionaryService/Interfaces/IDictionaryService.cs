using UniGate.DictionaryService.DTOs.Response;

namespace UniGate.DictionaryService.Interfaces;

public interface IDictionaryService
{
    public Task<List<EducationLevelDto>> GetEducationLevels();
}