using System.ComponentModel.DataAnnotations;

namespace UniGateAPI.Models;

public class ApplicantReference
{
    [Key] public Guid UserId { get; set; }
    public List<Admission> Admissions { get; set; } = [];
    public List<Guid> DocumentsIds { get; set; } = [];
}