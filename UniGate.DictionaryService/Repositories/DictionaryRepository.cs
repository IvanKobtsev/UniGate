using Microsoft.EntityFrameworkCore;
using UniGate.Common.Exceptions;
using UniGate.Common.Extensions;
using UniGate.Common.Utilities;
using UniGate.DictionaryService.Data;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Interfaces;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Repositories;

public class DictionaryRepository(ApplicationDbContext dbContext) : IDictionaryRepository
{
    public async Task StoreEducationLevels(List<EducationLevel> educationLevels)
    {
        foreach (var educationLevel in educationLevels)
        {
            var existingEducationLevel = await dbContext.EducationLevels
                .FirstOrDefaultAsync(el => el.EducationLevelId == educationLevel.EducationLevelId);

            if (existingEducationLevel == null)
                await dbContext.EducationLevels.AddAsync(educationLevel);
            else
                existingEducationLevel.Name = educationLevel.Name;
        }
    }

    public async Task StoreEducationPrograms(List<EducationProgram> educationPrograms)
    {
        var educationLevelsGuids = await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.EducationLevelId, el => el.Id);

        foreach (var educationProgram in educationPrograms)
        {
            var existingEducationProgram = await dbContext.EducationPrograms
                .FirstOrDefaultAsync(ep => ep.Id == educationProgram.Id);

            if (existingEducationProgram == null)
            {
                await dbContext.EducationPrograms.AddAsync(educationProgram);
            }
            else
            {
                existingEducationProgram.Name = educationProgram.Name;
                existingEducationProgram.Code = educationProgram.Code;
                existingEducationProgram.Language = educationProgram.Language;
                existingEducationProgram.EducationForm = educationProgram.EducationForm;
                existingEducationProgram.FacultyId = educationProgram.Faculty.Id;
                existingEducationProgram.EducationLevelId =
                    educationLevelsGuids[educationProgram.EducationLevel.EducationLevelId];
            }
        }
    }

    public async Task StoreFaculties(List<Faculty> faculties)
    {
        foreach (var faculty in faculties)
        {
            var existingFaculty = await dbContext.Faculties
                .FirstOrDefaultAsync(f => f.Id == faculty.Id);

            if (existingFaculty == null)
                await dbContext.Faculties.AddAsync(faculty);
            else
                existingFaculty.Name = faculty.Name;
        }
    }

    public async Task StoreEducationDocumentTypes(List<EducationDocumentType> educationDocumentTypes)
    {
        var educationLevelsGuids = await GetEducationLevelsIntToGuidDictionary();

        var educationLevelAccess = new List<EducationLevelAccess>();

        foreach (var educationDocumentType in educationDocumentTypes)
        {
            var existingEducationDocumentType = await dbContext.EducationDocumentTypes
                .FirstOrDefaultAsync(edt => edt.Id == educationDocumentType.Id);

            if (existingEducationDocumentType != null)
            {
                existingEducationDocumentType.Name = educationDocumentType.Name;
                continue;
            }

            await dbContext.EducationDocumentTypes.AddAsync(
                educationDocumentType);

            educationLevelAccess.AddRange(educationDocumentType.NextEducationLevels.Select(educationLevel =>
                new EducationLevelAccess
                {
                    Id = Guid.NewGuid(), EducationDocumentTypeId = educationDocumentType.Id,
                    EducationLevelId = educationLevelsGuids[educationLevel.EducationLevel.EducationLevelId]
                }));
        }

        await dbContext.EducationLevelAccesses.AddRangeAsync(educationLevelAccess);
    }

    public async Task<Guid> StartImport(ImportType importType)
    {
        var newImportId = Guid.NewGuid();

        await dbContext.ImportStates.AddAsync(new ImportState
        {
            Id = newImportId,
            ImportStartDateTime = DateTime.UtcNow,
            ImportStatus = ImportStatus.Importing,
            ImportType = importType
        });

        return newImportId;
    }

    public async Task FinishImport(Guid importId, ImportStatus importStatus)
    {
        var importToFinish = await dbContext.ImportStates.FindAsync(importId);

        if (importToFinish == null) throw new InternalServerException("Didn't find an import to finish");

        importToFinish.ImportEndDateTime = DateTime.UtcNow;
        importToFinish.ImportStatus = importStatus;
    }

    public async Task ClearDatabase()
    {
        await dbContext.EducationDocumentTypes.ExecuteDeleteAsync();
        await dbContext.EducationLevels.ExecuteDeleteAsync();
        await dbContext.EducationLevelAccesses.ExecuteDeleteAsync();
        await dbContext.EducationPrograms.ExecuteDeleteAsync();
        await dbContext.Faculties.ExecuteDeleteAsync();
    }

    public async Task<ImportState?> GetLastImportState()
    {
        return await dbContext.ImportStates.AsNoTracking().OrderByDescending(i => i.ImportStartDateTime)
            .FirstOrDefaultAsync();
    }

    public async Task<List<EducationLevel>> GetEducationLevels()
    {
        return await dbContext.EducationLevels.AsNoTracking().OrderBy(el => el.EducationLevelId).ToListAsync();
    }

    public async Task<List<EducationDocumentType>> GetEducationDocumentTypes()
    {
        var educationLevelsInts = await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.Id, el => el.EducationLevelId);

        return await dbContext.EducationDocumentTypes.AsNoTracking().Include(edt => edt.EducationLevel)
            .Include(edt => edt.NextEducationLevels)
            .ThenInclude(ela => ela.EducationLevel).AsSplitQuery()
            .OrderBy(edt => edt.Name).ToListAsync();
    }

    public async Task<List<Faculty>> GetFaculties()
    {
        return await dbContext.Faculties.AsNoTracking().OrderBy(f => f.Name).ToListAsync();
    }

    public async Task<PaginatedList<EducationProgram>> GetPaginatedEducationPrograms(int currentPage,
        int pageSize,
        Guid? facultyId, int? educationLevelId, string? educationForm, string? language,
        string? programSearch)
    {
        var query = dbContext.EducationPrograms.AsNoTracking()
            .Include(ep => ep.Faculty)
            .Include(ep => ep.EducationLevel)
            .AsSplitQuery();

        if (facultyId != null) query = query.Where(ep => ep.Faculty.Id == facultyId);
        if (educationLevelId != null) query = query.Where(ep => ep.EducationLevel.EducationLevelId == educationLevelId);
        if (!string.IsNullOrEmpty(educationForm)) query = query.Where(ep => ep.EducationForm == educationForm);
        if (!string.IsNullOrEmpty(language)) query = query.Where(ep => ep.Language == language);
        if (!string.IsNullOrEmpty(programSearch))
            query = query.Where(ep =>
                ep.Name.ToLower().Contains(programSearch.ToLower()) || ep.Code.Contains(programSearch));

        var totalCount = await query.CountAsync();

        var educationPrograms = await query.OrderBy(ep => ep.Name).Paginate(currentPage, pageSize)
            .ToListAsync();

        return new PaginatedList<EducationProgram>
        {
            Items = educationPrograms,
            TotalCount = totalCount,
            PageIndex = currentPage,
            PageSize = pageSize
        };
    }

    public async Task<List<ImportState>> GetImportStates()
    {
        return await dbContext.ImportStates.AsNoTracking().OrderByDescending(i => i.ImportStartDateTime).ToListAsync();
    }

    public async Task<Dictionary<int, Guid>> GetEducationLevelsIntToGuidDictionary()
    {
        return await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.EducationLevelId, el => el.Id);
    }

    public async Task<Dictionary<Guid, int>> GetEducationLevelsGuidToIntDictionary()
    {
        return await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.Id, el => el.EducationLevelId);
    }

    public async Task StoreEducationLevelAccesses(List<EducationLevelAccess> educationLevelAccesses)
    {
        await dbContext.EducationLevelAccesses.AddRangeAsync(educationLevelAccesses);
    }
}