using System.ComponentModel.DataAnnotations;

namespace UniGateAPI.Models;

public class DocumentReference
{
    public required ApplicantReference ApplicantReference { get; set; }
    [Key] public Guid DocumentId { get; set; }
}