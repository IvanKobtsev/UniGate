using Microsoft.EntityFrameworkCore;
using UniGate.Common.Enums;
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
                .FirstOrDefaultAsync(el => el.IntegerId == educationLevel.IntegerId);

            if (existingEducationLevel == null)
                await dbContext.EducationLevels.AddAsync(educationLevel);
            else
                existingEducationLevel.Name = educationLevel.Name;
        }
    }

    public async Task StoreEducationPrograms(List<EducationProgram> educationPrograms)
    {
        var educationLevelsGuids = await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.IntegerId, el => el.Id);

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
                    educationLevelsGuids[educationProgram.EducationLevel.IntegerId];
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

            educationLevelAccess.AddRange(educationDocumentType.NextEducationLevels);
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

        await dbContext.SaveChangesAsync();

        return newImportId;
    }

    public async Task FinishImport(Guid importId, ImportStatus importStatus)
    {
        var importToFinish = await dbContext.ImportStates.FindAsync(importId);

        if (importToFinish == null) throw new InternalServerException("Didn't find an import to finish");

        importToFinish.ImportEndDateTime = DateTime.UtcNow;
        importToFinish.ImportStatus = importStatus;

        await dbContext.SaveChangesAsync();
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

    public async Task<PaginatedList<EducationLevel>> GetPaginatedEducationLevels(int currentPage, int pageSize,
        string? levelName)
    {
        var query = dbContext.EducationLevels.AsNoTracking();

        if (!string.IsNullOrEmpty(levelName))
            query = query.Where(el => el.Name.ToLower().Contains(levelName.ToLower()));

        var totalCount = await query.CountAsync();

        var result = await query.OrderBy(el => el.IntegerId).Paginate(currentPage, pageSize).ToListAsync();

        return new PaginatedList<EducationLevel>
        {
            Items = result,
            TotalCount = totalCount,
            PageIndex = currentPage,
            PageSize = pageSize
        };
    }

    public async Task<PaginatedList<EducationProgram>> GetPaginatedEducationPrograms(int currentPage,
        int pageSize,
        Guid? facultyId, int? educationLevelId, string? educationForm, string? language,
        string? programNameOrCode)
    {
        var query = dbContext.EducationPrograms.AsNoTracking()
            .Include(ep => ep.Faculty)
            .Include(ep => ep.EducationLevel)
            .AsSplitQuery();

        if (facultyId != null) query = query.Where(ep => ep.Faculty.Id == facultyId);
        if (educationLevelId != null) query = query.Where(ep => ep.EducationLevel.IntegerId == educationLevelId);
        if (!string.IsNullOrEmpty(educationForm)) query = query.Where(ep => ep.EducationForm == educationForm);
        if (!string.IsNullOrEmpty(language)) query = query.Where(ep => ep.Language == language);
        if (!string.IsNullOrEmpty(programNameOrCode))
            query = query.Where(ep =>
                ep.Name.ToLower().Contains(programNameOrCode.ToLower()) ||
                ep.Code.Contains(programNameOrCode));

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

    public async Task<Dictionary<int, Guid>> GetEducationLevelsIntToGuidDictionary()
    {
        return await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.IntegerId, el => el.Id);
    }

    public async Task<Dictionary<Guid, int>> GetEducationLevelsGuidToIntDictionary()
    {
        return await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.Id, el => el.IntegerId);
    }

    public async Task<PaginatedList<Faculty>> GetPaginatedFaculties(int currentPage, int pageSize,
        string? facultyName)
    {
        var query = dbContext.Faculties.AsNoTracking();

        if (!string.IsNullOrEmpty(facultyName))
            query = query.Where(f => f.Name.ToLower().Contains(facultyName.ToLower()));

        var totalCount = await query.CountAsync();

        var result = await query.OrderBy(f => f.Name).Paginate(currentPage, pageSize).ToListAsync();

        return new PaginatedList<Faculty>
        {
            Items = result,
            TotalCount = totalCount,
            PageIndex = currentPage,
            PageSize = pageSize
        };
    }

    public async Task<PaginatedList<EducationDocumentType>> GetPaginatedEducationDocumentTypes(int currentPage,
        int pageSize,
        string? levelName)
    {
        var query = dbContext.EducationDocumentTypes.AsNoTracking();

        if (!string.IsNullOrEmpty(levelName))
            query = query.Where(edt => edt.Name.ToLower().Contains(levelName.ToLower()));

        var totalCount = await query.CountAsync();

        var result = await query.Include(edt => edt.EducationLevel)
            .Include(edt => edt.NextEducationLevels)
            .ThenInclude(edt => edt.EducationLevel).OrderBy(edt => edt.Name).Paginate(currentPage, pageSize)
            .ToListAsync();

        return new PaginatedList<EducationDocumentType>
        {
            Items = result,
            TotalCount = totalCount,
            PageIndex = currentPage,
            PageSize = pageSize
        };
    }

    public async Task<PaginatedList<ImportState>> GetImportStates(int currentPage,
        int pageSize, Sorting sorting)
    {
        var query = dbContext.ImportStates.AsNoTracking();

        query = sorting == Sorting.DateDesc
            ? query.OrderByDescending(i => i.ImportStartDateTime)
            : query.OrderBy(i => i.ImportStartDateTime);

        var totalCount = await query.CountAsync();

        var result = await query.Paginate(currentPage, pageSize).ToListAsync();

        return new PaginatedList<ImportState>
        {
            Items = result,
            TotalCount = totalCount,
            PageIndex = currentPage,
            PageSize = pageSize
        };
    }

    public async Task<EducationProgram?> GetEducationProgram(Guid programId)
    {
        return await dbContext.EducationPrograms.AsNoTracking().Include(ep => ep.EducationLevel)
            .FirstOrDefaultAsync(ep => ep.Id == programId);
    }

    public async Task<EducationDocumentType?> GetEducationDocumentType(Guid documentTypeId)
    {
        return await dbContext.EducationDocumentTypes.AsNoTracking().Include(ed => ed.EducationLevel)
            .Include(ed => ed.NextEducationLevels)
            .ThenInclude(ela => ela.EducationLevel).FirstOrDefaultAsync(dt => dt.Id == documentTypeId);
    }

    public async Task<List<EducationDocumentType>> GetAllEducationDocumentTypes()
    {
        return await dbContext.EducationDocumentTypes.AsNoTracking().Include(edt => edt.NextEducationLevels)
            .ThenInclude(ela => ela.EducationLevel).ToListAsync();
    }
}