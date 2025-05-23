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
            FacultyIdOfChosenProgram = programPreference.FacultyIdOfChosenProgram,
            Priority = programPreference.Priority
        };
    }

    public static List<ProgramPreferenceDto> ToDtos(this List<ProgramPreference> programPreferences)
    {
        return programPreferences.Select(ToDto).OrderBy(pp => pp.Priority).ToList();
    }
}