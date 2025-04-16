using UniGate.Common.DTOs;
using UniGate.Common.Enums;
using UniGate.Common.Exceptions;
using UniGate.DictionaryService.Data;
using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Interfaces;
using UniGate.DictionaryService.Mappers;

namespace UniGate.DictionaryService.Services;

public class ImportService(
    IExternalApiClient apiClient,
    IDictionaryRepository dictionaryRepository,
    ApplicationDbContext dbContext) : IImportService
{
    public async Task Import(ImportType importType)
    {
        if (importType == ImportType.SoftReset) await dictionaryRepository.ClearDatabase();

        var newImportId = await dictionaryRepository.StartImport(importType);

        try
        {
            var educationLevelDtos = await apiClient.ImportEducationLevelsAsync();

            await dictionaryRepository.StoreEducationLevels(educationLevelDtos.ToEducationLevels());

            await dbContext.SaveChangesAsync();

            var educationLevelsDictionary = await dictionaryRepository.GetEducationLevelsIntToGuidDictionary();

            var facultyDtos = await apiClient.ImportFacultiesAsync();

            await dictionaryRepository.StoreFaculties(facultyDtos.ToFaculties());

            var educationDocumentTypeDtos = await apiClient.ImportEducationDocumentTypesAsync();

            await dictionaryRepository.StoreEducationDocumentTypes(educationDocumentTypeDtos
                .ToEducationDocumentTypes(educationLevelsDictionary));

            var educationProgramsPaginatedList = await apiClient.ImportEducationProgramsAsync();
            var educationProgramsDtos = educationProgramsPaginatedList.Programs;
            var pageNumber = 2;

            do
            {
                educationProgramsPaginatedList = await apiClient.ImportEducationProgramsAsync(pageNumber++);
                educationProgramsDtos.AddRange(educationProgramsPaginatedList.Programs);
            } while (educationProgramsPaginatedList.Pagination.Current <
                     educationProgramsPaginatedList.Pagination.Count);

            await dictionaryRepository.StoreEducationPrograms(
                educationProgramsDtos.ToEducationPrograms(educationLevelsDictionary));

            await dictionaryRepository.FinishImport(newImportId, ImportStatus.Succeeded);
        }
        catch (NotFoundException exception)
        {
            await dictionaryRepository.FinishImport(newImportId, ImportStatus.Failed);

            throw;
        }
        catch (Exception exception)
        {
            await dictionaryRepository.FinishImport(newImportId, ImportStatus.Failed);

            throw;
        }
    }

    public async Task<PaginatedListDto<ImportStateDto>> GetImportHistory(int currentPage, int pageSize,
        Sorting sorting)
    {
        return (await dictionaryRepository.GetImportStates(currentPage, pageSize, sorting))
            .ToPaginatedListDto(items => items.ToDtos());
    }

    public async Task<ImportStateDto?> GetImportStatus()
    {
        return (await dictionaryRepository.GetLastImportState())?.ToDto();
    }

    public async Task<bool> IsImporting()
    {
        var lastImportState = await dictionaryRepository.GetLastImportState();

        return lastImportState is { ImportStatus: ImportStatus.Importing };
    }
}