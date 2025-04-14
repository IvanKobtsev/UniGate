using UniGateAPI.DTOs.Common;
using UniGateAPI.Models;

namespace UniGateAPI.Mappers;

public static class ApplicantMapper
{
    public static ApplicantLightDto ToLightDto(this ApplicantReference applicant)
    {
        return new ApplicantLightDto
        {
            UserId = applicant.UserId,
            FullName = applicant.FullName
        };
    }

    public static List<ApplicantLightDto> ToLightDto(this List<ApplicantReference> applicants)
    {
        return applicants.Select(ToLightDto).ToList();
    }
}