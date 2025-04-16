namespace UniGateAPI.DTOs;

public class CreateProgramPreferenceDto
{
    public required Guid ChosenProgram { get; set; }
    public int Priority { get; set; }
}