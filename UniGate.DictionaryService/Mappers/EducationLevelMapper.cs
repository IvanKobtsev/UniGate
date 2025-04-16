using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class EducationLevelMapper
{
    public static EducationLevel ToEducationLevel(this EducationLevelModel educationLevelModel)
    {
        return new EducationLevel
        {
            Id = Guid.NewGuid(),
            IntegerId = educationLevelModel.Id,
            Name = educationLevelModel.Name
        };
    }

    public static List<EducationLevel> ToEducationLevels(this List<EducationLevelModel> educationLevelDtos)
    {
        return educationLevelDtos.Select(ToEducationLevel).ToList();
    }

    public static EducationLevelModel ToExternalModel(this EducationLevel educationLevel)
    {
        return new EducationLevelModel
        {
            Id = educationLevel.IntegerId,
            Name = educationLevel.Name
        };
    }

    public static List<EducationLevelModel> ToExternalModels(this List<EducationLevel> educationLevels)
    {
        return educationLevels.Select(ToExternalModel).ToList();
    }

    public static EducationLevelDto ToDto(this EducationLevel educationLevel)
    {
        return new EducationLevelDto
        {
            Id = educationLevel.IntegerId,
            Name = educationLevel.Name
        };
    }

    public static List<EducationLevelDto> ToDtos(this List<EducationLevel> educationLevels)
    {
        return educationLevels.Select(ToDto).ToList();
    }
}