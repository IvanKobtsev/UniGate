using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class EducationProgramMapper
{
    public static EducationProgramModel ToExternalModel(this EducationProgram educationProgram)
    {
        return new EducationProgramModel
        {
            Id = educationProgram.Id,
            Name = educationProgram.Name,
            Code = educationProgram.Code,
            Language = educationProgram.Language,
            EducationForm = educationProgram.EducationForm,
            Faculty = educationProgram.Faculty.ToExternalModel(),
            EducationLevel = educationProgram.EducationLevel.ToExternalModel(),
            CreateTime = educationProgram.CreateTime
        };
    }

    public static List<EducationProgramModel> ToExternalModels(this List<EducationProgram> educationPrograms)
    {
        return educationPrograms.Select(ToExternalModel).ToList();
    }

    public static EducationProgram ToEducationProgram(this EducationProgramModel educationProgramModel,
        Dictionary<int, Guid> intToGuids)
    {
        return new EducationProgram
        {
            Id = educationProgramModel.Id,
            Name = educationProgramModel.Name,
            CreateTime = educationProgramModel.CreateTime.ToUniversalTime(),
            Code = educationProgramModel.Code,
            Language = educationProgramModel.Language,
            EducationForm = educationProgramModel.EducationForm,
            FacultyId = educationProgramModel.Faculty.Id,
            EducationLevelId = intToGuids[educationProgramModel.EducationLevel.Id]
        };
    }

    public static List<EducationProgram> ToEducationPrograms(this List<EducationProgramModel> educationPrograms,
        Dictionary<int, Guid> intToGuids)
    {
        return educationPrograms.Select(ep => ep.ToEducationProgram(intToGuids)).ToList();
    }

    public static EducationProgramDto ToDto(this EducationProgram educationProgram)
    {
        return new EducationProgramDto
        {
            Id = educationProgram.Id,
            Name = educationProgram.Name,
            Code = educationProgram.Code,
            Language = educationProgram.Language,
            EducationForm = educationProgram.EducationForm,
            Faculty = educationProgram.Faculty.ToDto(),
            EducationLevel = educationProgram.EducationLevel.ToDto()
        };
    }

    public static List<EducationProgramDto> ToDtos(this List<EducationProgram> educationPrograms)
    {
        return educationPrograms.Select(ToDto).ToList();
    }
}