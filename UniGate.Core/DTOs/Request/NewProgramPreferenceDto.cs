namespace UniGateAPI.DTOs;

public class NewProgramPreferenceDto
{
    public required Guid ChosenProgram { get; set; }
    public int Priority { get; set; }
}