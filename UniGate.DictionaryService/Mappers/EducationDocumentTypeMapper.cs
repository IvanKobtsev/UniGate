using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class EducationDocumentTypeMapper
{
    public static EducationDocumentType ToEducationDocumentType(this EducationDocumentTypeDto educationDocumentTypeDto,
        Dictionary<int, Guid> guidsDictionary)
    {
        return new EducationDocumentType
        {
            Id = educationDocumentTypeDto.Id,
            CreateTime = educationDocumentTypeDto.CreateTime.ToUniversalTime(),
            Name = educationDocumentTypeDto.Name,
            EducationLevelId = guidsDictionary[educationDocumentTypeDto.EducationLevel.Id]
        };
    }

    public static List<EducationDocumentType> ToEducationDocumentTypes(
        this List<EducationDocumentTypeDto> educationDocumentTypeDtos, Dictionary<int, Guid> guidsDictionary)
    {
        return educationDocumentTypeDtos.Select(el => el.ToEducationDocumentType(guidsDictionary)).ToList();
    }

    public static EducationDocumentTypeDto ToDto(this EducationDocumentType educationDocumentType,
        Dictionary<Guid, int> intsDictionary)
    {
        return new EducationDocumentTypeDto
        {
            Id = educationDocumentType.Id,
            CreateTime = educationDocumentType.CreateTime.ToUniversalTime(),
            Name = educationDocumentType.Name,
            EducationLevel = new EducationLevelDto
            {
                Id = intsDictionary[educationDocumentType.EducationLevelId],
                Name = educationDocumentType.EducationLevel.Name
            },
            NextEducationLevels = educationDocumentType.NextEducationLevels
                .Select(educationAccess => educationAccess.EducationLevel.ToDto()).ToList()
        };
    }

    public static List<EducationDocumentTypeDto> ToDtos(this List<EducationDocumentType> educationDocumentTypes,
        Dictionary<Guid, int> intsDictionary)
    {
        return educationDocumentTypes.Select(el => el.ToDto(intsDictionary)).ToList();
    }
}