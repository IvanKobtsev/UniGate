using UniGateAPI.DTOs.Common;
using UniGateAPI.Models;

namespace UniGateAPI.Mappers;

public static class ProgramPreferenceMapper
{
    public static ProgramPreferenceDto ToDto(this ProgramPreference programPreference)
    {
        return new ProgramPreferenceDto
        {
            Id = programPreference.Id,
            CreateTime = programPreference.CreateTime,
            ChosenProgramId = programPreference.ChosenProgramId,
            FacultyOfChosenProgramId = programPreference.FacultyOfChosenProgramId,
            Priority = programPreference.Priority
        };
    }

    public static List<ProgramPreferenceDto> ToDtos(this List<ProgramPreference> programPreferences)
    {
        return programPreferences.Select(ToDto).ToList();
    }
}