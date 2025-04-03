namespace UniGate.FileService.Models;

public class Passport : Document
{
    public string PassportNumber { get; set; } = string.Empty;
    public string BirthPlace { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public string IssuingAuthority { get; set; } = string.Empty;
}