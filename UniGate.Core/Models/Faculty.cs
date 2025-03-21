namespace UniGateAPI.Models;

public class Faculty
{
    Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    string Name { get; set; } = string.Empty;
    private List<Program> Programs { get; set; } = [];
    private List<Manager> Managers { get; set; } = [];
}