namespace UniGateAPI.DTOs;

public class ProgramPreferenceDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public required Guid ChosenProgram { get; set; }
    public int Priority { get; set; }
}