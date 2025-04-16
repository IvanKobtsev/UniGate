using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.DTOs.Dictionary;
using UniGate.DictionaryService.Models;

namespace UniGate.DictionaryService.Mappers;

public static class FacultyMapper
{
    public static Faculty ToFaculty(this FacultyModel facultyModel)
    {
        return new Faculty
        {
            Id = facultyModel.Id,
            CreateTime = facultyModel.CreateTime.ToUniversalTime(),
            Name = facultyModel.Name
        };
    }

    public static List<Faculty> ToFaculties(this List<FacultyModel> facultyDtos)
    {
        return facultyDtos.Select(ToFaculty).ToList();
    }

    public static FacultyModel ToExternalModel(this Faculty faculty)
    {
        return new FacultyModel
        {
            Id = faculty.Id,
            CreateTime = faculty.CreateTime,
            Name = faculty.Name
        };
    }

    public static List<FacultyModel> ToExternalModels(this List<Faculty> faculties)
    {
        return faculties.Select(ToExternalModel).ToList();
    }

    public static FacultyDto ToDto(this Faculty faculty)
    {
        return new FacultyDto
        {
            Id = faculty.Id,
            Name = faculty.Name
        };
    }

    public static List<FacultyDto> ToDtos(this List<Faculty> faculties)
    {
        return faculties.Select(ToDto).ToList();
    }
}