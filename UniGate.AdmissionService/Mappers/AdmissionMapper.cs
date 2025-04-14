using UniGateAPI.DTOs.Common;
using UniGateAPI.Models;

namespace UniGateAPI.Mappers;

public static class AdmissionMapper
{
    public static AdmissionLightDto ToLightDto(this Admission admission)
    {
        return new AdmissionLightDto
        {
            Id = admission.Id,
            CreateTime = admission.CreateTime,
            AdmissionType = admission.AdmissionType,
            Status = admission.Status,
            ApplicantId = admission.ApplicantId
        };
    }

    public static List<AdmissionLightDto> ToLightDtos(this List<Admission> admissions)
    {
        return admissions.Select(ToLightDto).ToList();
    }

    public static AdmissionDto ToDto(this Admission admission)
    {
        return new AdmissionDto
        {
            Id = admission.Id,
            CreateTime = admission.CreateTime,
            AdmissionType = admission.AdmissionType,
            Status = admission.Status,
            Applicant = admission.ApplicantReference.ToLightDto()
        };
    }

    public static List<AdmissionDto> ToDtos(this List<Admission> admissions)
    {
        return admissions.Select(ToDto).ToList();
    }

    public static AdmissionFullDto ToFullDto(this Admission admission)
    {
        return new AdmissionFullDto
        {
            Id = admission.Id,
            CreateTime = admission.CreateTime,
            AdmissionType = admission.AdmissionType,
            Status = admission.Status,
            Applicant = admission.ApplicantReference.ToLightDto(),
            ProgramPreferences = admission.ProgramPreferences.ToDtos()
        };
    }

    public static List<AdmissionFullDto> ToFullDtos(this List<Admission> admissions)
    {
        return admissions.Select(ToFullDto).ToList();
    }
}