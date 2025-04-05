using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class EducationLevelAccessMapper
{
    public static EducationLevelAccess ToEducationLevelAccess(this EducationLevelAccessDto educationLevelAccessDto)
    {
        return new EducationLevelAccess
        {
            Id = Guid.NewGuid(),
            EducationLevelId = educationLevelAccessDto.EducationLevelId,
            EducationDocumentTypeId = educationLevelAccessDto.EducationDocumentTypeId
        };
    }

    public static List<EducationLevelAccess> ToEducationLevelAccesses(
        this List<EducationLevelAccessDto> educationLevelAccessDtos)
    {
        return educationLevelAccessDtos.Select(ToEducationLevelAccess).ToList();
    }
}