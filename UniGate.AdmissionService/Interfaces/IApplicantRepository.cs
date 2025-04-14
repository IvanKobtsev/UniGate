using UniGate.Common.Utilities;
using UniGateAPI.DTOs.Common;
using UniGateAPI.Enums;
using UniGateAPI.Models;

namespace UniGateAPI.Interfaces;

public interface IApplicantRepository
{
    public Task<bool> AddApplicantReference(ApplicantReference applicant);
    public Task<ApplicantReference?> GetApplicantReference(Guid userId);
    public Task<ApplicantReference?> RetrieveApplicantReference(Guid userId);
    public Task<bool> RemoveApplicantReference(ApplicantReference applicant);
    public Task<Admission?> GetAdmissionById(Guid admissionId, bool includeChosenPrograms = false);
    public Task<Admission?> RetrieveAdmissionById(Guid admissionId);
    public Task<bool> AddAdmission(Admission admission);
    public Task<bool> AddProgramPreference(ProgramPreference programPreference);

    public Task<PaginatedList<AdmissionFullDto>> GetPaginatedAdmissions(string? name, Guid? programId,
        List<Guid> faculties, AdmissionStatus? admissionStatus,
        bool onlyNotTaken = false,
        bool onlyMine = false, Sorting sorting = Sorting.DateDesc,
        int page = 1, int pageSize = 10);
}