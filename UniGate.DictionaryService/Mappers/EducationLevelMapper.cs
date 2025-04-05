using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class EducationLevelMapper
{
    public static EducationLevel ToEducationLevel(this EducationLevelDto educationLevelDto)
    {
        return new EducationLevel
        {
            Id = Guid.NewGuid(),
            EducationLevelId = educationLevelDto.Id,
            Name = educationLevelDto.Name
        };
    }

    public static List<EducationLevel> ToEducationLevels(this List<EducationLevelDto> educationLevelDtos)
    {
        return educationLevelDtos.Select(ToEducationLevel).ToList();
    }

    public static EducationLevelDto ToDto(this EducationLevel educationLevel)
    {
        return new EducationLevelDto
        {
            Id = educationLevel.EducationLevelId,
            Name = educationLevel.Name
        };
    }

    public static List<EducationLevelDto> ToDtos(this List<EducationLevel> educationLevels)
    {
        return educationLevels.Select(ToDto).ToList();
    }
}