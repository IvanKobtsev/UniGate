namespace UniGateAPI.DTOs.Common;

public class ProgramPreferenceDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public required Guid ChosenProgramId { get; set; }
    public required Guid FacultyIdOfChosenProgram { get; set; }
    public int Priority { get; set; }
}