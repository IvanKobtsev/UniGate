using UniGate.Common.Utilities;
using UniGateAPI.DTOs.Response;
using UniGateAPI.Enums;

namespace UniGateAPI.Interfaces;

public interface IApplicantService
{
    public Task<Result> CreateAdmissionForApplicant(Guid userId, AdmissionType admissionType);
    public Task<Result> ChooseEducationProgramAsApplicant(Guid userId, Guid admissionId, Guid programId);

    public Task<Result<PaginatedAdmissionsList>> GetPaginatedAdmissions(string? name, Guid? programId,
        List<Guid> faculties, AdmissionStatus? admissionStatus,
        bool onlyNotTaken = false,
        bool onlyMine = false, Sorting sorting = Sorting.DateDesc,
        int page = 1, int pageSize = 10);
}