namespace UniGateAPI.Models;

public class ProgramPreference
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public required Admission Admission { get; set; }
    public required EducationProgram ChosenProgram { get; set; }
    public int Priority { get; set; }
}