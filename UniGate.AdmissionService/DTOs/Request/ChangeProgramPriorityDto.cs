using System.ComponentModel.DataAnnotations;

namespace UniGateAPI.DTOs.Request;

public class ChangeProgramPriorityDto
{
    [Range(1, int.MaxValue)] public int NewPriority { get; set; }
}