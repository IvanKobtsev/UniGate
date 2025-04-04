using Microsoft.EntityFrameworkCore;
using UniGate.DictionaryService.Data;
using UniGate.DictionaryService.DTOs.Response;
using UniGate.DictionaryService.Interfaces;
using UniGate.DictionaryService.Mappers;

namespace UniGate.DictionaryService.Repositories;

public class DictionaryRepository(ApplicationDbContext dbContext) : IDictionaryRepository
{
    public async Task StoreEducationLevels(List<EducationLevelDto> educationLevelDtos)
    {
        await dbContext.EducationLevels.AddRangeAsync(educationLevelDtos.ToEducationLevels());
    }

    public async Task CommitChanges()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<EducationLevelDto>> RetrieveEducationLevels()
    {
        return (await dbContext.EducationLevels.OrderBy(el => el.EducationLevelId).ToListAsync()).ToDtos();
    }

    // public async Task StoreEducationDocumentTypes(List<EducationDocumentTypeDto> educationDocumentTypesDtos)
    // {
    //     await dbContext.EducationLevels.AddRangeAsync(educationDocumentTypesDtos.ToEducationDocumentTypes());
    // }
}