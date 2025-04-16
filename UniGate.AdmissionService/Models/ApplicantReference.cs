using System.ComponentModel.DataAnnotations;

namespace UniGateAPI.Models;

public class ApplicantReference
{
    [Key] public Guid UserId { get; set; }
    [MaxLength(152)] public required string FullName { get; set; }
    public List<Admission> Admissions { get; set; } = [];
    public List<DocumentReference> Documents { get; set; } = [];
    public Guid? TypeIdOfUploadedEducationDocument { get; set; }
}