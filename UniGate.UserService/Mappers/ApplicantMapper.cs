using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Models;

namespace UniGate.UserService.Mappers;

public static class ApplicantMapper
{
    public static RegisterUserDto ToRegisterUserDto(this RegisterApplicantDto applicant)
    {
        return new RegisterUserDto
        {
            Email = applicant.Email,
            Password = applicant.Password,
            FirstName = applicant.FirstName,
            LastName = applicant.LastName,
            Patronymic = applicant.Patronymic,
            PhoneNumber = applicant.PhoneNumber
        };
    }

    public static ApplicantDto ToDto(this Applicant applicant)
    {
        return new ApplicantDto
        {
            Id = applicant.UserId,
            CreateTime = applicant.CreateTime,
            BirthDate = applicant.BirthDate,
            Citizenship = applicant.Citizenship,
            Gender = applicant.Gender
        };
    }

    public static Applicant ToApplicant(this RegisterApplicantDto applicant, Guid userId)
    {
        return new Applicant
        {
            UserId = userId,
            CreateTime = DateTime.UtcNow,
            BirthDate = applicant.BirthDate,
            Gender = applicant.Gender,
            Citizenship = applicant.Citizenship
        };
    }

    public static Applicant ToApplicant(this ApplicantDto applicantDto)
    {
        return new Applicant
        {
            UserId = applicantDto.Id,
            CreateTime = DateTime.UtcNow,
            BirthDate = applicantDto.BirthDate,
            Citizenship = applicantDto.Citizenship,
            Gender = applicantDto.Gender
        };
    }
}