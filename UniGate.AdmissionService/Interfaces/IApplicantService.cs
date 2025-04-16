using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGateAPI.DTOs.Response;
using UniGateAPI.Enums;

namespace UniGateAPI.Interfaces;

public interface IApplicantService
{
    public Task<Result<CreateAdmissionAsApplicantDto>> CreateAdmissionForApplicant(Guid userId,
        AdmissionType admissionType);

    public Task<Result> ChooseEducationProgramAsApplicant(Guid userId, Guid admissionId, Guid programId);

    public Task<Result> ChangePriorityOfProgramForApplicant(Guid userId, List<string> roles,
        Guid programPreferenceId,
        int newPriority);

    public Task<Result> DeleteProgramPreference(Guid userId, List<string> roles, Guid programPreferenceId);

    public Task<Result<PaginatedAdmissionsList>> GetPaginatedAdmissions(string? name, Guid? programId,
        List<Guid> faculties, AdmissionStatus? admissionStatus,
        bool onlyNotTaken = false,
        bool onlyMine = false, Sorting sorting = Sorting.DateDesc,
        int page = 1, int pageSize = 10);

    public Task<Result<AdmissionsOfApplicantDto>> GetAdmissionsOfUser(Guid userId);

    public Task<Result<ProgramPreferencesDto>> GetProgramPreferences(Guid userId, Guid admissionId);

    public Task<Result> CheckIfUserCanEditApplicantData(Guid userId, List<string> userRoles, Guid applicantId,
        bool onlyPersonalData = false);
}