namespace UniGate.DictionaryService.Models;

public class Faculty
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EducationProgram> Programs { get; set; } = [];
}