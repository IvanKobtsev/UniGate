namespace UniGateAPI.Models;

public class Document
{
    public Guid ApplicantId { get; set; }
    public required Applicant Applicant { get; set; }
    public Guid DocumentId { get; set; }
}