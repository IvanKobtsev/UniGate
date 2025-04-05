using Microsoft.EntityFrameworkCore;
using UniGate.Common.Exceptions;
using UniGate.DictionaryService.Data;
using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Interfaces;
using UniGate.DictionaryService.Mappers;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Repositories;

public class DictionaryRepository(ApplicationDbContext dbContext) : IDictionaryRepository
{
    public async Task StoreEducationLevels(List<EducationLevelDto> educationLevelDtos)
    {
        foreach (var educationLevel in educationLevelDtos)
        {
            var existingEducationLevel = await dbContext.EducationLevels
                .FirstOrDefaultAsync(el => el.EducationLevelId == educationLevel.Id);

            if (existingEducationLevel == null)
                await dbContext.EducationLevels.AddAsync(educationLevel.ToEducationLevel());
            else
                existingEducationLevel.Name = educationLevel.Name;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task StoreEducationPrograms(List<EducationProgramDto> educationProgramsDto)
    {
        var educationLevelsGuids = await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.EducationLevelId, el => el.Id);

        foreach (var educationProgram in educationProgramsDto)
        {
            var existingEducationProgram = await dbContext.EducationPrograms
                .FirstOrDefaultAsync(ep => ep.Id == educationProgram.Id);

            if (existingEducationProgram == null)
            {
                await dbContext.EducationPrograms.AddAsync(educationProgram.ToEducationProgram(educationLevelsGuids));
            }
            else
            {
                existingEducationProgram.Name = educationProgram.Name;
                existingEducationProgram.Code = educationProgram.Code;
                existingEducationProgram.Language = educationProgram.Language;
                existingEducationProgram.EducationForm = educationProgram.EducationForm;
                existingEducationProgram.FacultyId = educationProgram.Faculty.Id;
                existingEducationProgram.EducationLevelId = educationLevelsGuids[educationProgram.EducationLevel.Id];
            }
        }
    }

    public async Task StoreFaculties(List<FacultyDto> facultyDtos)
    {
        foreach (var facultyDto in facultyDtos)
        {
            var existingFaculty = await dbContext.Faculties
                .FirstOrDefaultAsync(f => f.Id == facultyDto.Id);

            if (existingFaculty == null)
                await dbContext.Faculties.AddAsync(facultyDto.ToFaculty());
            else
                existingFaculty.Name = facultyDto.Name;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task StoreEducationDocumentTypes(List<EducationDocumentTypeDto> educationDocumentTypeDtos)
    {
        var educationLevelsGuids = await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.EducationLevelId, el => el.Id);

        var educationLevelAccess = new List<EducationLevelAccess>();

        foreach (var educationDocumentType in educationDocumentTypeDtos)
        {
            var existingEducationDocumentType = await dbContext.EducationDocumentTypes
                .FirstOrDefaultAsync(edt => edt.Id == educationDocumentType.Id);

            if (existingEducationDocumentType != null)
            {
                existingEducationDocumentType.Name = educationDocumentType.Name;
                continue;
            }

            await dbContext.EducationDocumentTypes.AddAsync(
                educationDocumentType.ToEducationDocumentType(educationLevelsGuids));

            educationLevelAccess.AddRange(educationDocumentType.NextEducationLevels.Select(educationLevelDto =>
                new EducationLevelAccess
                {
                    Id = Guid.NewGuid(), EducationDocumentTypeId = educationDocumentType.Id,
                    EducationLevelId = educationLevelsGuids[educationLevelDto.Id]
                }));
        }

        await dbContext.EducationLevelAccesses.AddRangeAsync(educationLevelAccess);
    }

    public async Task<List<EducationLevelDto>> RetrieveEducationLevels()
    {
        return (await dbContext.EducationLevels.AsNoTracking().OrderBy(el => el.EducationLevelId).ToListAsync())
            .ToDtos();
    }

    public async Task<List<EducationDocumentTypeDto>> RetrieveEducationDocumentTypes()
    {
        var educationLevelsInts = await dbContext.EducationLevels.AsNoTracking()
            .ToDictionaryAsync(el => el.Id, el => el.EducationLevelId);

        return (await dbContext.EducationDocumentTypes.AsNoTracking().Include(edt => edt.EducationLevel)
            .Include(edt => edt.NextEducationLevels)
            .ThenInclude(ela => ela.EducationLevel).AsSplitQuery()
            .OrderBy(edt => edt.Name).ToListAsync()).ToDtos(educationLevelsInts);
    }

    public async Task<EducationProgramsDto> RetrieveEducationPrograms(int currentPage = 1, int pageSize = 10,
        Guid? facultyId = null, Guid? educationLevelId = null, string? educationForm = null, string? language = null,
        string? programSearch = null)
    {
        var query = dbContext.EducationPrograms.AsNoTracking()
            .Include(ep => ep.Faculty)
            .Include(ep => ep.EducationLevel)
            .AsSplitQuery();

        if (facultyId != null) query = query.Where(ep => ep.Faculty.Id == facultyId);
        if (educationLevelId != null) query = query.Where(ep => ep.EducationLevelId == educationLevelId);
        if (!string.IsNullOrEmpty(educationForm)) query = query.Where(ep => ep.EducationForm == educationForm);
        if (!string.IsNullOrEmpty(language)) query = query.Where(ep => ep.Language == language);
        if (!string.IsNullOrEmpty(programSearch))
            query = query.Where(ep => ep.Name.ToLower().Contains(programSearch.ToLower()));

        var totalCount = await query.CountAsync();

        var items = await query.OrderBy(ep => ep.Name)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new EducationProgramsDto
        {
            Pagination = new PaginationDto
            {
                Count = totalCount,
                Current = currentPage,
                Size = pageSize
            },
            Programs = items.ToDtos()
        };
    }

    public async Task<List<FacultyDto>> RetrieveFaculties()
    {
        return (await dbContext.Faculties.AsNoTracking().OrderBy(f => f.Name).ToListAsync()).ToDtos();
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
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<ImportStateDto>> RetrieveImportStates()
    {
        return (await dbContext.ImportStates.AsNoTracking().OrderByDescending(i => i.ImportStartDateTime).ToListAsync())
            .ToDtos();
    }

    public async Task<ImportStateDto?> RetrieveLastImportState()
    {
        return (await dbContext.ImportStates.AsNoTracking().OrderByDescending(i => i.ImportStartDateTime)
            .FirstOrDefaultAsync())?.ToDto();
    }

    public async Task StoreEducationLevelAccesses(List<EducationLevelAccessDto> educationLevelAccessDtos)
    {
        await dbContext.EducationLevelAccesses.AddRangeAsync(educationLevelAccessDtos.ToEducationLevelAccesses());
    }
}