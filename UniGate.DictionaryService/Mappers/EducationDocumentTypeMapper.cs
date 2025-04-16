using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class EducationDocumentTypeMapper
{
    public static EducationDocumentType ToEducationDocumentType(
        this EducationDocumentTypeModel educationDocumentTypeModel,
        Dictionary<int, Guid> guidsDictionary)
    {
        return new EducationDocumentType
        {
            Id = educationDocumentTypeModel.Id,
            CreateTime = educationDocumentTypeModel.CreateTime.ToUniversalTime(),
            Name = educationDocumentTypeModel.Name,
            EducationLevelId = guidsDictionary[educationDocumentTypeModel.EducationLevel.Id],
            NextEducationLevels = educationDocumentTypeModel.NextEducationLevels
                .Select(educationLevelModel => new EducationLevelAccess
                {
                    EducationLevelId = guidsDictionary[educationLevelModel.Id],
                    EducationDocumentTypeId = educationDocumentTypeModel.Id
                }).ToList()
        };
    }

    public static List<EducationDocumentType> ToEducationDocumentTypes(
        this List<EducationDocumentTypeModel> educationDocumentTypeDtos, Dictionary<int, Guid> guidsDictionary)
    {
        return educationDocumentTypeDtos.Select(el => el.ToEducationDocumentType(guidsDictionary)).ToList();
    }

    public static EducationDocumentTypeModel ToExternalModel(this EducationDocumentType educationDocumentType,
        Dictionary<Guid, int> intsDictionary)
    {
        return new EducationDocumentTypeModel
        {
            Id = educationDocumentType.Id,
            CreateTime = educationDocumentType.CreateTime.ToUniversalTime(),
            Name = educationDocumentType.Name,
            EducationLevel = new EducationLevelModel
            {
                Id = intsDictionary[educationDocumentType.EducationLevelId],
                Name = educationDocumentType.EducationLevel.Name
            },
            NextEducationLevels = educationDocumentType.NextEducationLevels
                .Select(educationAccess => educationAccess.EducationLevel.ToExternalModel()).ToList()
        };
    }

    public static List<EducationDocumentTypeModel> ToExternalModels(
        this List<EducationDocumentType> educationDocumentTypes,
        Dictionary<Guid, int> intsDictionary)
    {
        return educationDocumentTypes.Select(el => el.ToExternalModel(intsDictionary)).ToList();
    }

    public static EducationDocumentTypeDto ToDto(this EducationDocumentType educationDocumentType)
    {
        return new EducationDocumentTypeDto
        {
            Id = educationDocumentType.Id,
            Name = educationDocumentType.Name,
            EducationLevel = educationDocumentType.EducationLevel.ToDto(),
            NextEducationLevels = educationDocumentType.NextEducationLevels.Select(nel => nel.EducationLevel).ToList()
                .ToDtos()
        };
    }

    public static List<EducationDocumentTypeDto> ToDtos(this List<EducationDocumentType> educationDocumentTypes)
    {
        return educationDocumentTypes.Select(ToDto).ToList();
    }
}