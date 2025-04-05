using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class FacultyMapper
{
    public static Faculty ToFaculty(this FacultyDto facultyDto)
    {
        return new Faculty
        {
            Id = facultyDto.Id,
            CreateTime = facultyDto.CreateTime.ToUniversalTime(),
            Name = facultyDto.Name
        };
    }

    public static List<Faculty> ToFaculties(this List<FacultyDto> facultyDtos)
    {
        return facultyDtos.Select(ToFaculty).ToList();
    }

    public static FacultyDto ToDto(this Faculty faculty)
    {
        return new FacultyDto
        {
            Id = faculty.Id,
            CreateTime = faculty.CreateTime,
            Name = faculty.Name
        };
    }

    public static List<FacultyDto> ToDtos(this List<Faculty> faculties)
    {
        return faculties.Select(ToDto).ToList();
    }
}