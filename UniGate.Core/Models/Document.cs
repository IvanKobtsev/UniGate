using UniGateAPI.Enums;

namespace UniGateAPI.Models;

public abstract class Document
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public required Applicant OwnerApplicant { get; set; }
    public List<UploadedFile> Files { get; set; } = [];
    public string DocumentType { get; set; } = string.Empty;
}