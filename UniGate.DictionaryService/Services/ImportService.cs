using UniGate.Common.Exceptions;
using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Services;

public class ImportService(IExternalApiClient apiClient, IDictionaryRepository dictionaryRepository) : IImportService
{
    public async Task Import(ImportType importType)
    {
        if (importType == ImportType.SoftReset) await dictionaryRepository.ClearDatabase();

        var newImportId = await dictionaryRepository.StartImport(importType);

        try
        {
            var educationLevelDtos = await apiClient.ImportEducationLevelsAsync();

            await dictionaryRepository.StoreEducationLevels(educationLevelDtos);

            var facultyDtos = await apiClient.ImportFacultiesAsync();

            await dictionaryRepository.StoreFaculties(facultyDtos);

            var educationDocumentTypeDtos = await apiClient.ImportEducationDocumentTypesAsync();

            await dictionaryRepository.StoreEducationDocumentTypes(educationDocumentTypeDtos);

            var educationProgramsPaginatedList = await apiClient.ImportEducationProgramsAsync();
            var educationProgramsDtos = educationProgramsPaginatedList.Programs;
            var pageNumber = 2;

            do
            {
                educationProgramsPaginatedList = await apiClient.ImportEducationProgramsAsync(pageNumber++);
                educationProgramsDtos.AddRange(educationProgramsPaginatedList.Programs);
            } while (educationProgramsPaginatedList.Pagination.Current <
                     educationProgramsPaginatedList.Pagination.Count);

            await dictionaryRepository.StoreEducationPrograms(educationProgramsDtos);

            await dictionaryRepository.FinishImport(newImportId, ImportStatus.Succeeded);
        }
        catch (ServiceUnavailableException exception)
        {
            await dictionaryRepository.FinishImport(newImportId, ImportStatus.Failed);

            throw new ServiceUnavailableException("Import failed: service responded with null");
        }
        catch (Exception exception)
        {
            await dictionaryRepository.FinishImport(newImportId, ImportStatus.Failed);

            throw;
        }
    }

    public async Task<List<ImportStateDto>> GetImportHistory()
    {
        return await dictionaryRepository.RetrieveImportStates();
    }

    public async Task<ImportStateDto?> GetImportStatus()
    {
        return await dictionaryRepository.RetrieveLastImportState();
    }

    public async Task<bool> IsImporting()
    {
        var lastImportState = await dictionaryRepository.RetrieveLastImportState();

        return lastImportState is { ImportStatus: ImportStatus.Importing };
    }
}