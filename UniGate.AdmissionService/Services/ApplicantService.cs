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
    IManagerRepository managerRepository,
    HmacHttpClient hmacHttpClient)
    : IApplicantService
{
    public async Task<Result<CreateAdmissionAsApplicantDto>> CreateAdmissionForApplicant(Guid userId,
        AdmissionType admissionType)
    {
        var foundApplicant = await applicantRepository.GetApplicantReferenceById(userId);

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
        var applicantReference = await applicantRepository.GetApplicantReferenceById(userId);

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
            throw new InternalServerException("An unexpected error happened when creating new program preference");

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
        var foundApplicant = await applicantRepository.RetrieveApplicantReferenceById(userId, true);

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

    public async Task<Result> ChangePriorityOfProgramForApplicant(Guid userId, List<string> userRoles,
        Guid programPreferenceId,
        int newPriority)
    {
        var foundProgramPreference = await admissionRepository.RetrieveProgramPreferenceById(programPreferenceId);

        if (foundProgramPreference == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Program preference not found"
            };

        var foundAdmission = await admissionRepository.RetrieveAdmissionById(foundProgramPreference.AdmissionId, true);

        if (foundAdmission == null)
            throw new InvalidOperationException("Invalid data in DB: Program reference with id = " +
                                                programPreferenceId + " is linked to the non-existent admission");

        var canEditResult = await CheckIfUserCanEditApplicantData(userId, userRoles,
            foundAdmission.ApplicantReference.UserId);

        if (canEditResult.IsFailed) return canEditResult;

        if (newPriority > foundAdmission.ProgramPreferences.Count)
            return new Result
            {
                Code = HttpCode.BadRequest,
                Message = "New priority can't be greater than the number of currently chosen programs"
            };

        if (newPriority == foundProgramPreference.Priority)
            return new Result
            {
                Code = HttpCode.BadRequest,
                Message = "New priority can't be equal to the current one"
            };

        var oldPriority = foundProgramPreference.Priority;
        foundProgramPreference.Priority = newPriority;
        foundProgramPreference.Admission.LastUpdateTime = DateTime.UtcNow;

        foundAdmission.ProgramPreferences.ForEach(pp =>
        {
            if (pp.Priority > oldPriority && pp.Id != programPreferenceId)
                --pp.Priority;
            if (pp.Priority >= newPriority && pp.Id != programPreferenceId)
                ++pp.Priority;
        });

        var sortResult = await SortProgramPreferences(foundProgramPreference.AdmissionId);
        if (!sortResult)
            throw new InternalServerException("Failed to sort program preferences after changing priority");

        await dbContext.SaveChangesAsync();

        return new Result();
    }

    public async Task<Result> DeleteProgramPreference(Guid userId, List<string> roles, Guid programPreferenceId)
    {
        var programPreference = await admissionRepository.RetrieveProgramPreferenceById(programPreferenceId, true);

        if (programPreference == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Program preference not found"
            };


        var checkResult = await CheckIfUserCanEditApplicantData(userId, roles, programPreference.Admission.ApplicantId);

        if (checkResult.IsFailed) return checkResult;

        var deleteResult = await admissionRepository.RemoveProgramPreference(programPreference);
        if (!deleteResult)
            throw new InternalServerException("An unexpected error happened when deleting program preference");

        var sortResult = await SortProgramPreferences(programPreference.AdmissionId);
        if (!sortResult)
            throw new InternalServerException(
                "An unexpected error happened when sorting program preferences after deletion");

        await dbContext.SaveChangesAsync();

        return new Result();
    }

    public async Task<Result> CheckIfUserCanEditApplicantData(Guid userId, List<string> userRoles, Guid applicantId,
        bool onlyPersonalData = false)
    {
        var foundApplicant = await applicantRepository.RetrieveApplicantReferenceById(applicantId, true);

        if (foundApplicant == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Applicant not found"
            };

        if (onlyPersonalData) return await CanManagerEditApplicantPersonalData(userId, userRoles, foundApplicant);

        return CanUserEditSpecifiedApplicant(userId, userRoles, foundApplicant);
    }

    private static Result CanUserEditSpecifiedApplicant(Guid userId, List<string> userRoles,
        ApplicantReference applicant)
    {
        if (userRoles.Contains("Admins") || userRoles.Contains("ChiefManager")) return new Result();

        if (userRoles.Contains("Manager") && applicant.Admissions.All(a => a.ManagerId != userId))
            return new Result
            {
                Code = HttpCode.Forbidden,
                Message = "The specified applicant can't be edited by current user in any way"
            };
        if (userRoles.Contains("Applicant") && userId != applicant.UserId)
            return new Result
            {
                Code = HttpCode.Forbidden,
                Message = "Applicants can't be edited by other applicants"
            };

        return new Result();
    }

    private async Task<Result> CanManagerEditApplicantPersonalData(Guid userId, List<string> userRoles,
        ApplicantReference applicant)
    {
        var initialResult = CanUserEditSpecifiedApplicant(userId, userRoles, applicant);

        if (!initialResult.IsFailed || !userRoles.Contains("Manager")) return initialResult;

        var foundManager = await managerRepository.GetManagerReferenceById(userId);

        if (foundManager == null)
            throw new InvalidOperationException("Invalid data in DB: there should be manager with Id = " + userId);

        return applicant.Admissions.Any(a => a.ProgramPreferences.Any(pp =>
            pp.Priority == 1 && pp.FacultyIdOfChosenProgram == foundManager.AssignedFacultyId))
            ? new Result()
            : initialResult;
    }

    private async Task<bool> SortProgramPreferences(Guid admissionId)
    {
        var foundAdmission = await admissionRepository.RetrieveAdmissionById(admissionId, true);

        if (foundAdmission == null)
            return false;

        var sortedList = foundAdmission.ProgramPreferences.OrderBy(pp => pp.Priority).ToList();

        for (var i = 0; i < sortedList.Count; i++)
            sortedList[i].Priority = i + 1;

        return true;
    }
}