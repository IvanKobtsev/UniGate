using UniGate.DictionaryService.DTOs.Response;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Services;

public class DictionaryService(IDictionaryRepository dictionaryRepository) : IDictionaryService
{
    public Task<List<EducationLevelDto>> GetEducationLevels()
    {
        return dictionaryRepository.RetrieveEducationLevels();
    }
}