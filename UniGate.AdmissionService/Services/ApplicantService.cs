using UniGate.Common.DTOs;
using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGateAPI.Data;
using UniGateAPI.DTOs.Response;
using UniGateAPI.Enums;
using UniGateAPI.Interfaces;
using UniGateAPI.Models;

namespace UniGateAPI.Services;

public class ApplicantService(
    ApplicationDbContext dbContext,
    IApplicantRepository applicantRepository,
    IConfiguration config)
    : IApplicantService
{
    public async Task<Result> CreateAdmissionForApplicant(Guid userId, AdmissionType admissionType)
    {
        var foundApplicant = await applicantRepository.GetApplicantReference(userId);

        if (foundApplicant == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Applicant not found"
            };

        var result = await applicantRepository.AddAdmission(new Admission
        {
            ApplicantId = userId,
            CreateTime = DateTime.UtcNow,
            LastUpdateTime = DateTime.UtcNow,
            Status = AdmissionStatus.Created,
            AdmissionType = admissionType
        });

        if (!result)
            return new Result
            {
                Code = HttpCode.BadRequest,
                Message = "Admission of that type already exists"
            };

        await dbContext.SaveChangesAsync();

        return new Result();
    }

    public async Task<Result<PaginatedAdmissionsList>> GetPaginatedAdmissions(string? name, Guid? programId,
        List<Guid> faculties, AdmissionStatus? admissionStatus, bool onlyNotTaken, bool onlyMine, Sorting sorting,
        int page, int pageSize)
    {
        var result = await applicantRepository.GetPaginatedAdmissions(
            name,
            programId,
            faculties,
            admissionStatus,
            onlyNotTaken,
            onlyMine,
            sorting,
            page, pageSize);

        if (page > result.PagesCount)
            return new Result<PaginatedAdmissionsList>
            {
                Code = HttpCode.BadRequest,
                Message = "Exceeded total number of pages"
            };

        return new Result<PaginatedAdmissionsList>
        {
            Data = new PaginatedAdmissionsList
            {
                Admissions = result.Items,
                Pagination = new PaginationDto
                {
                    Count = result.PagesCount,
                    Current = page,
                    Size = pageSize
                }
            }
        };
    }

    public async Task<Result> ChooseEducationProgramAsApplicant(Guid userId, Guid admissionId, Guid programId)
    {
        var foundAdmission = await applicantRepository.GetAdmissionById(admissionId, true);

        if (foundAdmission == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Admission not found"
            };

        if (foundAdmission.ApplicantId != userId)
            return new Result
            {
                Code = HttpCode.Forbidden,
                Message = "The specified admission doesn't belong to current user"
            };

        if (foundAdmission.ProgramPreferences.Count >= int.Parse(config["AllowedNumberOfPrograms"] ??
                                                                 throw new InvalidOperationException(
                                                                     "Missing allowed number of education programs in configuration")))
            return new Result
            {
                Code = HttpCode.BadRequest,
                Message = "You have already chosen the maximum number of programs"
            };

        // CHECK FOR PROGRAM EXISTENCE

        var result = await applicantRepository.AddProgramPreference(new ProgramPreference
        {
            Id = Guid.NewGuid(),
            CreateTime = DateTime.UtcNow,
            AdmissionId = admissionId,
            ChosenProgramId = programId,
            Priority = foundAdmission.ProgramPreferences.Count + 1
        });

        if (!result)
            return new Result
            {
                Code = HttpCode.BadRequest,
                Message = "You have already chosen this program"
            };

        await dbContext.SaveChangesAsync();

        return new Result();
    }
}