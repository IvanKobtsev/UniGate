using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Services;

public class ImportService(IExternalApiClient apiClient, IDictionaryRepository dictionaryRepository) : IImportService
{
    public async Task Import()
    {
        var educationLevelDtos = await apiClient.ImportEducationLevelsAsync();

        await dictionaryRepository.StoreEducationLevels(educationLevelDtos);
    }
}