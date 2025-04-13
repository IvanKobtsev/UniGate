using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniGate.Common.Enums;

namespace UniGate.UserService.Models;

public class Applicant
{
    [Key] public Guid UserId { get; set; }

    public DateTime CreateTime { get; set; }

    [ForeignKey("UserId")] public User User { get; set; }

    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    [MinLength(1)] [MaxLength(255)] public required string Citizenship { get; set; }
}