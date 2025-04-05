using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class EducationProgramMapper
{
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
            EducationLevel = educationProgram.EducationLevel.ToDto(),
            CreateTime = educationProgram.CreateTime
        };
    }

    public static List<EducationProgramDto> ToDtos(this List<EducationProgram> educationPrograms)
    {
        return educationPrograms.Select(ToDto).ToList();
    }

    public static EducationProgram ToEducationProgram(this EducationProgramDto educationProgramDto,
        Dictionary<int, Guid> intToGuids)
    {
        return new EducationProgram
        {
            Id = educationProgramDto.Id,
            Name = educationProgramDto.Name,
            CreateTime = educationProgramDto.CreateTime.ToUniversalTime(),
            Code = educationProgramDto.Code,
            Language = educationProgramDto.Language,
            EducationForm = educationProgramDto.EducationForm,
            FacultyId = educationProgramDto.Faculty.Id,
            EducationLevelId = intToGuids[educationProgramDto.EducationLevel.Id]
        };
    }

    public static List<EducationProgram> ToEducationPrograms(this List<EducationProgramDto> educationPrograms,
        Dictionary<int, Guid> intToGuids)
    {
        return educationPrograms.Select(ep => ep.ToEducationProgram(intToGuids)).ToList();
    }
}