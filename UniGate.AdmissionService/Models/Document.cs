namespace UniGateAPI.Models;

public class Document
{
    public Guid ApplicantId { get; set; }
    public required ApplicantReference ApplicantReference { get; set; }
    public Guid DocumentId { get; set; }
}