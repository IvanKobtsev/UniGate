using Newtonsoft.Json;
using UniGate.Common.DTOs;
using UniGate.Common.Enums;
using UniGate.Common.Exceptions;
using UniGate.Common.HMAC;
using UniGate.Common.Utilities;
using UniGateAPI.Data;
using UniGateAPI.DTOs.Response;
using UniGateAPI.Enums;
using UniGateAPI.Interfaces;
using UniGateAPI.Mappers;
using UniGateAPI.Models;

namespace UniGateAPI.Services;

public class ApplicantService(
    ApplicationDbContext dbContext,
    IApplicantRepository applicantRepository,
    IConfiguration config,
    IAdmissionRepository admissionRepository,
    HmacHttpClient hmacHttpClient)
    : IApplicantService
{
    public async Task<Result<CreateAdmissionAsApplicantDto>> CreateAdmissionForApplicant(Guid userId,
        AdmissionType admissionType)
    {
        var foundApplicant = await applicantRepository.GetApplicantReference(userId);

        if (foundApplicant == null)
            return new Result<CreateAdmissionAsApplicantDto>
            {
                Code = HttpCode.NotFound,
                Message = "Applicant not found"
            };

        var result = await admissionRepository.AddAdmission(new Admission
        {
            ApplicantId = userId,
            CreateTime = DateTime.UtcNow,
            LastUpdateTime = DateTime.UtcNow,
            Status = AdmissionStatus.Created,
            AdmissionType = admissionType
        });

        if (result == null)
            return new Result<CreateAdmissionAsApplicantDto>
            {
                Code = HttpCode.BadRequest,
                Message = "Admission of that type already exists"
            };

        await dbContext.SaveChangesAsync();

        return new Result<CreateAdmissionAsApplicantDto>
        {
            Data = new CreateAdmissionAsApplicantDto
            {
                CreatedAdmissionId = result.Value
            }
        };
    }

    public async Task<Result<PaginatedAdmissionsList>> GetPaginatedAdmissions(string? name, Guid? programId,
        List<Guid> faculties, AdmissionStatus? admissionStatus, bool onlyNotTaken, bool onlyMine, Sorting sorting,
        int page, int pageSize)
    {
        var result = await admissionRepository.GetPaginatedAdmissions(
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
        var applicantReference = await applicantRepository.GetApplicantReference(userId);

        if (applicantReference == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Applicant reference not found"
            };

        var foundAdmission = await admissionRepository.GetAdmissionById(admissionId, true);

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

        Guid? alreadyChosenProgram = null;
        var educationDocumentTypeId = applicantReference.TypeIdOfUploadedEducationDocument;

        if (foundAdmission.ProgramPreferences.Count > 0)
        {
            if (foundAdmission.ProgramPreferences.Any(pp => pp.ChosenProgramId == programId))
                return new Result { Code = HttpCode.BadRequest, Message = "You have already chosen specified program" };

            alreadyChosenProgram = foundAdmission.ProgramPreferences[0].ChosenProgramId;
        }

        var response =
            await hmacHttpClient.GetAsync("https://localhost:44358/api/v1/validation/programs/" + programId +
                                          "?alreadyChosenProgram=" + alreadyChosenProgram +
                                          "&educationDocumentTypeId=" + educationDocumentTypeId);

        var receivedString = await response.Content.ReadAsStringAsync();

        var validationResult = JsonConvert.DeserializeObject<Result<Guid>>(receivedString);

        if (validationResult == null)
            throw new InternalServerException("Failed to deserialize response from validation service");

        if (validationResult.IsFailed) return validationResult;

        var addResult = await admissionRepository.AddProgramPreference(new ProgramPreference
        {
            Id = Guid.NewGuid(),
            CreateTime = DateTime.UtcNow,
            AdmissionId = admissionId,
            ChosenProgramId = programId,
            FacultyIdOfChosenProgram = validationResult.Data,
            Priority = foundAdmission.ProgramPreferences.Count + 1
        });

        if (!addResult)
            return new Result
            {
                Code = HttpCode.InternalServerError,
                Message = "An unexpected error happened when adding program preference"
            };

        await dbContext.SaveChangesAsync();

        return new Result();
    }

    public async Task<Result<ProgramPreferencesDto>> GetProgramPreferences(Guid userId, Guid admissionId)
    {
        var foundAdmission = await admissionRepository.GetAdmissionById(admissionId, true);

        if (foundAdmission == null)
            return new Result<ProgramPreferencesDto>
            {
                Code = HttpCode.NotFound,
                Message = "Admission not found"
            };

        if (foundAdmission.ApplicantId != userId)
            return new Result<ProgramPreferencesDto>
            {
                Code = HttpCode.Forbidden,
                Message = "The specified admission doesn't belong to current user"
            };

        return new Result<ProgramPreferencesDto>
        {
            Data = new ProgramPreferencesDto
            {
                ProgramPreferences = foundAdmission.ProgramPreferences.ToDtos()
            }
        };
    }

    public async Task<Result<AdmissionsOfApplicantDto>> GetAdmissionsOfUser(Guid userId)
    {
        var foundApplicant = await applicantRepository.RetrieveApplicantReference(userId, true);

        if (foundApplicant == null)
            return new Result<AdmissionsOfApplicantDto>
            {
                Code = HttpCode.NotFound,
                Message = "Applicant not found"
            };

        var paidAdmission = foundApplicant.Admissions.FirstOrDefault(a => a.AdmissionType == AdmissionType.Paid);
        var budgetaryAdmission =
            foundApplicant.Admissions.FirstOrDefault(a => a.AdmissionType == AdmissionType.Budgetary);

        return new Result<AdmissionsOfApplicantDto>
        {
            Data = new AdmissionsOfApplicantDto
            {
                PaidAdmission = paidAdmission?.ToMyDto(),
                BudgetaryAdmission = budgetaryAdmission?.ToMyDto()
            }
        };
    }
}